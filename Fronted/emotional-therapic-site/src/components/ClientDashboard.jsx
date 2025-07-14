import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchAppointments, cancelAppointment } from '../redux/thunk';
import { setSuccess, setError } from '../redux/appointmentsSlice';
import { Box, Paper, Button, Typography, Alert } from '@mui/material';
import AppointmentScheduler from './AppointmentScheduler';

function ClientDashboard() {
  const [open, setOpen] = useState(false);
  const appointments = useSelector(state => state.appointments.appointments);
  const clientId = useSelector(state => state.user.userInfo?.id);
  const userName = useSelector(state => state.user.userInfo?.name);
  const error = useSelector(state => state.appointments.error);
  const success = useSelector(state => state.appointments.success);
  const dispatch = useDispatch();

  useEffect(() => {
    if (clientId) {
      dispatch(fetchAppointments(clientId));
    }
  }, [clientId, dispatch]);

  const handleDeleteAppointment = (appointmentId) => {
    dispatch(clearStatus());
    dispatch(cancelAppointment({ appointmentId, clientId }))
      .unwrap()
      .then(() => dispatch(setSuccess('Appointment deleted successfully!')))
      .catch((err) => {
        dispatch(setError(err?.message || 'An error occurred while deleting the appointment.'));
      });
  };

  return (
    <div>
      <h2>Hello {userName || ''}</h2>
      <Button variant="contained" onClick={() => setOpen(true)}>Schedule Appointment</Button>
      <AppointmentScheduler open={open} handleClose={() => setOpen(false)} />

      {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
      {success && <Alert severity="success" sx={{ mt: 2 }}>{success}</Alert>}

      {appointments.length > 0 ? (
        <>
          <Typography variant="h5" sx={{ mb: 2 }}>Your appointments:</Typography>
          {appointments.map((app, index) => (
            <Paper key={index} sx={{ p: 2, mb: 2 }}>
              <Typography variant="h6"><strong>Therapist Name:</strong> {app.name}</Typography>
              <Typography><strong>Date:</strong> {new Date(app.date).toLocaleDateString()}</Typography>
              <Typography><strong>Time:</strong> {new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</Typography>
              <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
                <Button
                  variant="outlined"
                  color="error"
                  onClick={() => handleDeleteAppointment(app.id)}
                >
                  Delete Appointment
                </Button>
              </Box>
            </Paper>
          ))}
        </>
      ) : (
        <Typography sx={{ mt: 2, fontSize: 18 }}>
          You have no appointments at the moment. You can schedule a new appointment from the button above.
        </Typography>
      )}
    </div>
  );
}
export default ClientDashboard;
