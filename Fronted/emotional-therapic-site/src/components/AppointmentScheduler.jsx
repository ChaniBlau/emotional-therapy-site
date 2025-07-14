// AppointmentScheduler.jsx
import React, { useState, useEffect } from "react";
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
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
      alert("The appointment was successfully scheduled!");
      if (role === "Client") {
        dispatch(fetchAppointments(clientId));
      }
      handleClose();
    } else {
      alert("An error occurred while scheduling the appointment.");
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Schedule an appointment</DialogTitle>
      <DialogContent>
        <FormControl fullWidth margin="normal">
          <InputLabel>Filter by</InputLabel>
          <Select
            value={mode}
            onChange={(e) => dispatch(setMode(e.target.value))}
            label="Filter by"
          >
            <MenuItem value="therapist">According to therapist</MenuItem>
            <MenuItem value="date">By date</MenuItem>
          </Select>
        </FormControl>

        <FormControl fullWidth margin="normal">
          <InputLabel>Select a therapist</InputLabel>
          <Select
            value={selectedTherapist}
            onChange={(e) => dispatch(setSelectedTherapist(e.target.value))}
            label="Select a therapist"
          >
            {(mode === "therapist" ? therapists : availableTherapists).map((therapist) => (
              <MenuItem key={therapist.id} value={therapist.id}>
                {therapist.firstName} {therapist.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <LocalizationProvider dateAdapter={AdapterDateFns}>
  <DatePicker
    label="Choose Date"
    value={localDate}
    onChange={(newValue) => {
      setLocalDate(newValue);
      dispatch(setSelectedDate(newValue));
      if (mode === "date") {
        dispatch(fetchAvailableTherapistsByDate(newValue));
      }
    }}
    renderInput={(params) => <TextField {...params} fullWidth margin="normal" />}
  />
</LocalizationProvider>
        <FormControl fullWidth margin="normal">
          <InputLabel>Select time</InputLabel>
          <Select
            value={selectedTime}
            onChange={(e) => dispatch(setSelectedTime(e.target.value))}
            label="Choose a time"
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
        <Button onClick={handleClose}>Cancel</Button>
        <Button onClick={handleSchedule} variant="contained" color="primary">
         Make an appointment
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AppointmentScheduler;
