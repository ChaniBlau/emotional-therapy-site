import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchAppointments, cancelAppointment } from '../redux/thunk';
import { setSuccess, setError, clearStatus } from '../redux/appointmentsSlice';
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

  const handleDeleteAppointment = (appointmentId, clientId) => {
    console.log("=== Delete Debug Info ===");
    console.log("appointmentId:", appointmentId);
    console.log("clientId:", clientId);
    console.log("appointmentId type:", typeof appointmentId);
    console.log("clientId type:", typeof clientId);

    if (!appointmentId || !clientId) {
      console.error("Missing appointmentId or clientId");
      dispatch(setError('Missing appointment or client information'));
      return;
    }

    // נוודא שהמזהה הוא מספר
    const numericAppointmentId = parseInt(appointmentId);
    if (isNaN(numericAppointmentId)) {
      console.error("Invalid appointmentId:", appointmentId);
      dispatch(setError('Invalid appointment ID'));
      return;
    }

    dispatch(clearStatus());
    dispatch(cancelAppointment({ 
      appointmentId: numericAppointmentId, 
      clientId: clientId.toString().trim()
    }))
      .unwrap()
      .then(() => {
        dispatch(setSuccess('Appointment deleted successfully!'));
        // רענן את רשימת התורים לאחר מחיקה מוצלחת
        dispatch(fetchAppointments(clientId));
      })
      .catch((err) => {
        console.error("Cancel appointment error:", err);
        dispatch(setError(err?.message || 'An error occurred while deleting the appointment.'));
      });
  };

  return (
    <div>
      <h2>Hello {userName || ''}</h2>
      <Button variant="contained" onClick={() => setOpen(true)}>Schedule Appointment</Button>
      <AppointmentScheduler open={open} handleClose={() => setOpen(false)} />

      {error && (
  <Alert severity="error" sx={{ mb: 2 }}>
    {(() => {
      if (typeof error === 'string') return error;
      if (error?.message) return error.message;
      if (error?.data?.message) return error.data.message;
      return 'An unexpected error occurred';
    })()}
  </Alert>
)}
      {success && <Alert severity="success" sx={{ mt: 2 }}>{success}</Alert>}

      {appointments.length > 0 ? (
        <>
          <Typography variant="h5" sx={{ mb: 2 }}>Your appointments:</Typography>
          {appointments.map((app, index) => {
            // בדיקה מה יש בתור
            console.log(`Appointment ${index}:`, app);
            
            // נסה למצוא את מזהה התור בכל הדרכים האפשריות
            const appointmentId = app.appointmentId || app.id || app.code || app.Code;
            
            return (
              <Paper key={index} sx={{ p: 2, mb: 2 }}>
                <Typography variant="h6"><strong>Therapist Name:</strong> {app.name}</Typography>
                <Typography><strong>Date:</strong> {new Date(app.date).toLocaleDateString()}</Typography>
                <Typography><strong>Time:</strong> {new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</Typography>
                
                {/* הצגת מידע debug */}
                <Typography variant="caption" color="text.secondary">
                </Typography>
                
                <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
                  <Button
                    variant="outlined"
                    color="error"
                    disabled={!appointmentId} // נבטל את הכפתור אם אין מזהה
                    onClick={() => handleDeleteAppointment(appointmentId, clientId)}
                  >
                    Delete Appointment
                  </Button>
                </Box>
              </Paper>
            );
          })}
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