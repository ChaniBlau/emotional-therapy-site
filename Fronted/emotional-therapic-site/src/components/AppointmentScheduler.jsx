// AppointmentScheduler.jsx
import React, { useState, useEffect } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
} from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import {
  setSelectedDate,
  setSelectedTherapist,
  setSelectedTime,
  setMode,
} from "../redux/appointmentsSlice";
import {
  fetchTherapists,
  scheduleAppointment,
  fetchAppointments,
  fetchAvailableHours,
  fetchAvailableTherapistsByDate,
  cancelAppointment
} from '../redux/thunk';

const AppointmentScheduler = ({ open, handleClose }) => {
  const dispatch = useDispatch();
  const {
    therapists,
    availableTherapists,
    selectedTherapist,
    selectedDate,
    selectedTime,
    mode,
    availableHours,
  } = useSelector((state) => state.appointments);
  const clientId = useSelector((state) => state.user.client?.id);
  const role = useSelector((state) => state.user.role);

  const [localDate, setLocalDate] = useState("");

  useEffect(() => {
    dispatch(fetchTherapists());
  }, [dispatch]);

  useEffect(() => {
    if (selectedTherapist && selectedDate) {
      dispatch(fetchAvailableHours({ therapistId: selectedTherapist, date: selectedDate }));
    }
  }, [dispatch, selectedTherapist, selectedDate]);

  const handleDateChange = (e) => {
    const date = e.target.value;
    setLocalDate(date);
    dispatch(setSelectedDate(date));
    if (mode === "date") {
      dispatch(fetchAvailableTherapistsByDate(date));
    }
  };

  const handleSchedule = async () => {
    if (!selectedTherapist || !selectedDate || !selectedTime || !clientId) return;

    const resultAction = await dispatch(
      scheduleAppointment({
        therapistId: selectedTherapist,
        date: selectedDate,
        time: selectedTime,
        clientId,
      })
    );

    if (scheduleAppointment.fulfilled.match(resultAction)) {
      alert("התור נקבע בהצלחה!");
      if (role === "Client") {
        dispatch(fetchAppointments(clientId));
      }
      handleClose();
    } else {
      alert("אירעה שגיאה בקביעת התור.");
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>קביעת תור</DialogTitle>
      <DialogContent>
        <FormControl fullWidth margin="normal">
          <InputLabel>סנן לפי</InputLabel>
          <Select
            value={mode}
            onChange={(e) => dispatch(setMode(e.target.value))}
            label="סנן לפי"
          >
            <MenuItem value="therapist">לפי מטפל</MenuItem>
            <MenuItem value="date">לפי תאריך</MenuItem>
          </Select>
        </FormControl>

        <FormControl fullWidth margin="normal">
          <InputLabel>בחר מטפל</InputLabel>
          <Select
            value={selectedTherapist}
            onChange={(e) => dispatch(setSelectedTherapist(e.target.value))}
            label="בחר מטפל"
          >
            {(mode === "therapist" ? therapists : availableTherapists).map((therapist) => (
              <MenuItem key={therapist.id} value={therapist.id}>
                {therapist.firstName} {therapist.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <TextField
          fullWidth
          margin="normal"
          type="date"
          label="בחר תאריך"
          InputLabelProps={{ shrink: true }}
          value={localDate}
          onChange={handleDateChange}
        />

        <FormControl fullWidth margin="normal">
          <InputLabel>בחר שעה</InputLabel>
          <Select
            value={selectedTime}
            onChange={(e) => dispatch(setSelectedTime(e.target.value))}
            label="בחר שעה"
          >
            {availableHours.map((time, index) => (
              <MenuItem key={index} value={time}>
                {time}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </DialogContent>

      <DialogActions>
        <Button onClick={handleClose}>ביטול</Button>
        <Button onClick={handleSchedule} variant="contained" color="primary">
          קבע תור
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AppointmentScheduler;
