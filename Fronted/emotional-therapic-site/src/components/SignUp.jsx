// SignUp.jsx
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { signUpClient } from '../redux/thunk';
import { setUser } from '../redux/userSlice';
import {
  Paper, TextField, Button, Typography, Alert, CircularProgress, Snackbar
} from '@mui/material';
import { useNavigate } from 'react-router-dom';

const SignUp = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { loading, error } = useSelector((state) => state.client);

  const [form, setForm] = useState({
    id: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    yearOfBirth: '',
    email: '',
    city: '',
  });

  const [showToast, setShowToast] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const clientData = {
      Id: form.id.trim() || crypto.randomUUID(),
      FirstName: form.firstName.trim(),
      LastName: form.lastName.trim(),
      PhoneNumber: form.phoneNumber.trim(),
      YearOfBirth: parseInt(form.yearOfBirth, 10),
      Email: form.email.trim(),
      TherapistId: null,
      City: form.city.trim(),
    };

    dispatch(signUpClient(clientData))
      .unwrap()
      .then(() => {
        dispatch(setUser({
          id: clientData.Id,
          name: clientData.FirstName + " " + clientData.LastName,
          role: 'client'
        }));

        setShowToast(true);

        setTimeout(() => {
          navigate('/client-dashboard');
        }, 1500);
      });
  };

  return (
    <>
      <Paper elevation={4} sx={{ p: 4, width: 350, borderRadius: 3 }}>
        <Typography variant="h4" align="center" color="primary" gutterBottom>
          Sign Up
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField name="id" label="ID" value={form.id} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="firstName" label="First Name" value={form.firstName} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="lastName" label="Last Name" value={form.lastName} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="phoneNumber" label="Phone Number" value={form.phoneNumber} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="yearOfBirth" label="Year of Birth" type="number" value={form.yearOfBirth} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="email" label="Email" type="email" value={form.email} onChange={handleChange} fullWidth margin="normal" required />
          <TextField name="city" label="City" value={form.city} onChange={handleChange} fullWidth margin="normal" required />

          <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 2, py: 1.3 }} disabled={loading}>
            {loading ? <CircularProgress size={24} /> : 'Sign Up'}
          </Button>

          {error && (
            <Alert severity="error" sx={{ mt: 2, fontWeight: 'bold' }}>
              {error}
            </Alert>
          )}
        </form>
      </Paper>

      <Snackbar
        open={showToast}
        autoHideDuration={3000}
        onClose={() => setShowToast(false)}
        message="Registration successful!"
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      />
    </>
  );
};

export default SignUp;
