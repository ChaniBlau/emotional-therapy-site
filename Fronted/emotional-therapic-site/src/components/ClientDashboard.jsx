import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { cancelAppointment, scheduleAppointment } from '../redux/thunk';
import { Box, Paper, Button, Typography, Alert, Modal, ToggleButtonGroup, ToggleButton } from '@mui/material';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { StaticDatePicker } from '@mui/x-date-pickers/StaticDatePicker';
import dayjs from 'dayjs';

const style = {
  position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', width: 450,
  bgcolor: 'background.paper', border: '2px solid #000', boxShadow: 24, p: 4, maxHeight: '90vh', overflowY: 'auto',
};

function ClientDashboard() {
  const appointments = useSelector(state => state.appointments.appointments);
  const clientId = useSelector(state => state.user.userInfo?.id);
  const userName = useSelector(state => state.user.userInfo?.name);
  const dispatch = useDispatch();

  // Modal state
  const [openDatePickerModal, setOpenDatePickerModal] = useState(false);
  const [selectedDate, setSelectedDate] = useState(null);
  const [schedulingTherapistId, setSchedulingTherapistId] = useState(null);
  const [availableTimeSlots, setAvailableTimeSlots] = useState([]);
  const [selectedTimeSlot, setSelectedTimeSlot] = useState(null);

  // UI state
  const [loading, setLoading] = useState(false);
  const [fetchingTimeSlots, setFetchingTimeSlots] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  // פותח את המודל לקביעת תור
  const handleOpenDatePicker = (therapistId) => {
    if (!clientId) {
      setError("Please log in to schedule an appointment.");
      return;
    }
    setSchedulingTherapistId(therapistId);
    setOpenDatePickerModal(true);
    setSelectedDate(null);
    setSelectedTimeSlot(null);
    setAvailableTimeSlots([]);
    setError(null);
    setSuccess(null);
  };

  // סוגר את המודל
  const handleCloseDatePicker = () => {
    setOpenDatePickerModal(false);
    setSelectedDate(null);
    setSelectedTimeSlot(null);
    setAvailableTimeSlots([]);
    setError(null);
    setSuccess(null);
  };

  // שינוי תאריך במודל
  const handleDateChange = async (date) => {
    setSelectedDate(date);
    setSelectedTimeSlot(null);
    setAvailableTimeSlots([]);
    if (date && schedulingTherapistId && clientId) {
      setFetchingTimeSlots(true);
      setError(null);
      try {
        const formattedDate = date.format('YYYY-MM-DD');
        const response = await fetch(
          `/api/Client/AvailableTimeSlots?therapistId=${schedulingTherapistId}&date=${formattedDate}`
        );
        if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.message || 'Failed to fetch available time slots.');
        }
        const slots = await response.json();
        setAvailableTimeSlots(slots);
        if (slots.length === 0) {
          setError('No available time slots for this date.');
        }
      } catch (err) {
        setError(err.message || 'An error occurred while fetching time slots.');
      } finally {
        setFetchingTimeSlots(false);
      }
    }
  };

  // שינוי שעה במודל
  const handleTimeSlotChange = (event, newTime) => {
    setSelectedTimeSlot(newTime);
  };

  // קביעת תור בפועל (redux thunk)
  const handleScheduleAppointment = () => {
    if (!selectedDate || !selectedTimeSlot || !schedulingTherapistId || !clientId) {
      setError("Please select a date and time, and ensure client ID is available.");
      return;
    }
    setLoading(true);
    setError(null);
    setSuccess(null);

    const formattedDate = selectedDate.format('YYYY-MM-DD');
    const formattedTime = selectedTimeSlot;

    dispatch(scheduleAppointment({
      therapistId: schedulingTherapistId,
      date: formattedDate,
      time: formattedTime,
      clientId
    }))
      .unwrap()
      .then(() => {
        setSuccess('Appointment scheduled successfully!');
        handleCloseDatePicker();
      })
      .catch((err) => {
        setError(err?.message || 'An error occurred while scheduling the appointment.');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  // מחיקת תור (redux thunk)
  const handleDeleteAppointment = (appointmentId) => {
    setError(null);
    setSuccess(null);
    dispatch(cancelAppointment({ appointmentId, clientId }))
      .unwrap()
      .then(() => setSuccess('Appointment deleted successfully!'))
      .catch((err) => {
        setError(err?.message || 'An error occurred while deleting the appointment.');
      });
  };

  return (
    <div>
      <h2>Hello {userName || ''}</h2>
      <Typography variant="h5" sx={{ mb: 2 }}>Your appointments:</Typography>
      {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
      {success && <Alert severity="success" sx={{ mt: 2 }}>{success}</Alert>}

      {appointments.length > 0 ? (
        appointments.map((app, index) => (
          <Paper key={index} sx={{ p: 2, mb: 2 }}>
            <Typography variant="h6"><strong>Therapist Name:</strong> {app.name}</Typography>
            <Typography><strong>Date:</strong> {new Date(app.date).toLocaleDateString()}</Typography>
            <Typography><strong>Time:</strong> {new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</Typography>
            <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
              <Button
                variant="contained"
                color="primary"
                onClick={() => handleOpenDatePicker(app.id)}
                disabled={!clientId}
              >
                Schedule New Appointment
              </Button>
              <Button
                variant="outlined"
                color="error"
                onClick={() => handleDeleteAppointment(app.id)}
              >
                Delete Appointment
              </Button>
            </Box>
          </Paper>
        ))
      ) : (
        <Typography>No appointments found.</Typography>
      )}

      <Modal open={openDatePickerModal} onClose={handleCloseDatePicker}>
        <Box sx={style}>
          <Typography variant="h6" sx={{ mb: 2 }}>Select Date and Time</Typography>
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <StaticDatePicker
              displayStaticWrapperAs="desktop"
              value={selectedDate}
              onChange={handleDateChange}
              minDate={dayjs()}
            />
          </LocalizationProvider>
          {fetchingTimeSlots ? (
            <Typography sx={{ mt: 2 }}>Loading time slots...</Typography>
          ) : (
            availableTimeSlots.length > 0 && (
              <ToggleButtonGroup
                value={selectedTimeSlot}
                exclusive
                onChange={handleTimeSlotChange}
                sx={{ mt: 2, mb: 2, flexWrap: 'wrap' }}
              >
                {availableTimeSlots.map((slot, idx) => (
                  <ToggleButton key={idx} value={slot}>
                    {slot}
                  </ToggleButton>
                ))}
              </ToggleButtonGroup>
            )
          )}
          <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
            <Button
              variant="contained"
              color="primary"
              onClick={handleScheduleAppointment}
             // disabled={loading }
            >
              Schedule Appointment
            </Button>
            <Button variant="outlined" onClick={handleCloseDatePicker}>
              Cancel
            </Button>
          </Box>
        </Box>
      </Modal>
    </div>
  );
}

export default ClientDashboard;