import React, { useState, useEffect } from "react";
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { format, isValid, parseISO } from 'date-fns';
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
  Alert,
  CircularProgress,
} from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import {
  setSelectedDate,
  setSelectedTherapist,
  setSelectedTime,
  setMode,
  clearStatus,
} from "../redux/appointmentsSlice";
import {
  fetchTherapists,
  scheduleAppointment,
  fetchAppointments,
  fetchAvailableHours,
  fetchAvailableTherapistsByDate,
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
    loading,
    error,
    success
  } = useSelector((state) => state.appointments);
  
  const clientId = useSelector((state) => state.user.userInfo?.id);
  const role = useSelector((state) => state.user.role);

  const [validationError, setValidationError] = useState('');

  useEffect(() => {
    if (open) {
      dispatch(fetchTherapists());
      dispatch(clearStatus());
      setValidationError('');
    }
  }, [dispatch, open]);

  useEffect(() => {
    if (selectedDate) {
      if (mode === "date") {
        dispatch(fetchAvailableTherapistsByDate(selectedDate));
      }
      
      if (selectedTherapist && selectedDate) {
        dispatch(fetchAvailableHours({ therapistId: selectedTherapist, date: selectedDate }));
      }
    }
  }, [dispatch, selectedDate, selectedTherapist, mode]);

  const handleDateChange = (newValue) => {
    try {
      if (newValue && isValid(newValue)) {
        const formattedDate = format(newValue, 'yyyy-MM-dd');
        dispatch(setSelectedDate(formattedDate));
        dispatch(setSelectedTherapist(''));
        dispatch(setSelectedTime(''));
      } else {
        dispatch(setSelectedDate(''));
        dispatch(setSelectedTherapist(''));
        dispatch(setSelectedTime(''));
      }
    } catch (error) {
      console.error('Invalid date:', error);
      dispatch(setSelectedDate(''));
      dispatch(setSelectedTherapist(''));
      dispatch(setSelectedTime(''));
    }
  };

  const handleTherapistChange = (therapistId) => {
    dispatch(setSelectedTherapist(therapistId));
    dispatch(setSelectedTime('')); 
  };

  const handleModeChange = (newMode) => {
    dispatch(setMode(newMode));
    dispatch(setSelectedDate(''));
    dispatch(setSelectedTherapist(''));
    dispatch(setSelectedTime(''));
  };

  const handleSchedule = async () => {
    if (!selectedTherapist || !selectedDate || !selectedTime || !clientId) {
      setValidationError('Please fill in all fields');
      return;
    }

    setValidationError('');

    try {
      const resultAction = await dispatch(
        scheduleAppointment({
          therapistId: selectedTherapist,
          date: selectedDate,
          time: selectedTime,
          clientId,
        })
      );

      if (scheduleAppointment.fulfilled.match(resultAction)) {
        if (role === "client") {
          dispatch(fetchAppointments(clientId));
        }
        
        handleCloseDialog();
      }
    } catch (error) {
      console.error('Error scheduling appointment:', error);
      setValidationError('Failed to schedule appointment. Please try again.');
    }
  };

  const handleCloseDialog = () => {
    dispatch(setSelectedTherapist(''));
    dispatch(setSelectedDate(''));
    dispatch(setSelectedTime(''));
    dispatch(clearStatus());
    setValidationError('');
    handleClose();
  };

  const dateValue = selectedDate ? parseISO(selectedDate) : null;

  const formatTime = (timeString) => {
    try {
      if (typeof timeString === 'string') {
        const timeParts = timeString.split(':');
        const hours = parseInt(timeParts[0]);
        const minutes = parseInt(timeParts[1]);
        
        if (!isNaN(hours) && !isNaN(minutes)) {
          const date = new Date();
          date.setHours(hours, minutes, 0, 0);
          return format(date, 'HH:mm');
        }
      }
      return timeString;
    } catch (error) {
      return timeString;
    }
  };

  return (
    <Dialog open={open} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
      <DialogTitle>Schedule an appointment</DialogTitle>
      <DialogContent>
        {success && (
          <Alert severity="success" sx={{ mb: 2 }}>
            {success}
          </Alert>
        )}

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {typeof error === 'string' ? error : (error.message || 'An error occurred')}
          </Alert>
        )}
        
        {validationError && (
          <Alert severity="warning" sx={{ mb: 2 }}>
            {validationError}
          </Alert>
        )}

        <FormControl fullWidth margin="normal">
          <InputLabel>Filter by</InputLabel>
          <Select
            value={mode}
            onChange={(e) => handleModeChange(e.target.value)}
            label="Filter by"
          >
            <MenuItem value="therapist">According to therapist</MenuItem>
            <MenuItem value="date">By date</MenuItem>
          </Select>
        </FormControl>

        <LocalizationProvider dateAdapter={AdapterDateFns}>
          <DatePicker
            label="Choose Date"
            value={dateValue}
            onChange={handleDateChange}
            minDate={new Date()}
            format="dd/MM/yyyy"
            slotProps={{
              textField: {
                fullWidth: true,
                margin: "normal",
                error: false,
              }
            }}
          />
        </LocalizationProvider>

        <FormControl fullWidth margin="normal">
          <InputLabel>Select a therapist</InputLabel>
          <Select
            value={selectedTherapist}
            onChange={(e) => handleTherapistChange(e.target.value)}
            label="Select a therapist"
            disabled={mode === "date" && !selectedDate}
          >
            {(mode === "therapist" ? therapists : availableTherapists).map((therapist) => (
              <MenuItem key={therapist.id} value={therapist.id}>
                {therapist.firstName} {therapist.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl fullWidth margin="normal">
          <InputLabel>Select time</InputLabel>
          <Select
            value={selectedTime}
            onChange={(e) => dispatch(setSelectedTime(e.target.value))}
            label="Choose a time"
            disabled={!selectedTherapist || !selectedDate}
          >
            {availableHours && availableHours.length > 0 ? (
              availableHours.map((time, index) => (
                <MenuItem key={index} value={time}>
                  {formatTime(time)}
                </MenuItem>
              ))
            ) : (
              selectedTherapist && selectedDate && !loading && (
                <MenuItem disabled>No available hours for this date</MenuItem>
              )
            )}
          </Select>
        </FormControl>

        {loading && (
          <div style={{ display: 'flex', justifyContent: 'center', marginTop: '16px' }}>
            <CircularProgress />
          </div>
        )}
      </DialogContent>

      <DialogActions>
        <Button onClick={handleCloseDialog}>Cancel</Button>
        <Button 
          onClick={handleSchedule} 
          variant="contained" 
          color="primary"
          disabled={loading || !selectedTherapist || !selectedDate || !selectedTime}
        >
          {loading ? 'Scheduling...' : 'Make an appointment'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AppointmentScheduler;