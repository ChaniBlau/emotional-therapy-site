import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { signUpClient } from '../redux/thunk';
import { Box, Paper, TextField, Button, Typography, Alert, CircularProgress } from '@mui/material';

const SignUp = () => {
  const dispatch = useDispatch();
  const { loading, error } = useSelector((state) => state.client);

  const [form, setForm] = useState({
    id: '', 
    firstName: '',
    lastName: '',
    phoneNumber: '',
    yearOfBirth: '',
    email: '',
    city: '', // הוסף את שדה ה-city
  });

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
      TherapistId: "",
      City: form.city.trim(), // הוסף את שדה ה-city
    };
    console.log(clientData);
    dispatch(signUpClient(clientData));
  };

  return (
      <Paper elevation={4} sx={{ p: 4, width: 350, borderRadius: 3 }}>
        <Typography variant="h4" align="center" color="primary" gutterBottom>
          Sign Up
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            name="id"
            label="ID"
            variant="outlined"
            value={form.id}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="firstName"
            label="First Name"
            variant="outlined"
            value={form.firstName}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="lastName"
            label="Last Name"
            variant="outlined"
            value={form.lastName}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="phoneNumber"
            label="Phone Number"
            variant="outlined"
            value={form.phoneNumber}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="yearOfBirth"
            label="Year of Birth"
            variant="outlined"
            type="number"
            min="1900"
            max={new Date().getFullYear()}
            value={form.yearOfBirth}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="email"
            label="Email"
            variant="outlined"
            type="email"
            value={form.email}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="city"
            label="City"
            variant="outlined"
            value={form.city}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <Button
            type="submit"
            variant="contained"
            color="primary"
            className="w-100 mt-3"
            sx={{ py: 1.3, fontWeight: 'bold', fontSize: 17, borderRadius: 2 }}
            disabled={loading}
          >
            {loading ? <CircularProgress size={24} /> : 'Sign Up'}
          </Button>
          {error && (
            <Alert severity="error" sx={{ mt: 2, fontWeight: 'bold' }}>
              {error}
            </Alert>
          )}
        </form>
      </Paper>
  );
};

export default SignUp;
