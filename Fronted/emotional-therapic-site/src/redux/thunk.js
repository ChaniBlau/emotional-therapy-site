import { createAsyncThunk } from '@reduxjs/toolkit';
import { setAppointments } from './appointmentsSlice';
import { setUser } from './userSlice';

export const loginUser = createAsyncThunk(
  'user/loginUser',
  async ({ id, name }, { dispatch, rejectWithValue }) => {
    try {
      const response = await fetch(`http://localhost:5222/api/Appointments/GetAllBusyAppointmentsForUser?id=${id}&name=${name}`);
      if (!response.ok) {
        throw new Error("User not found");
      }

      const data = await response.json();
      console.log(data);
      const isTherapist = data.length > 0 && data[0].Email !== undefined;
      dispatch(setUser({ id, name, role: isTherapist ? "therapist" : "client" }));
      dispatch(setAppointments(data));

      return data;
    } catch (err) {
      return rejectWithValue(err.message);
    }
  }
);

export const signUpClient = createAsyncThunk(
  'client/signUpClient',
  async (clientData, { rejectWithValue }) => {
    try {
      const response = await fetch('http://localhost:5222/api/Appointments/CreateNewClient', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify(clientData),
});
      if (!response.ok) {
        throw new Error("Registration failed");
      }
      const data = await response.json();
      return data;
    } catch (err) {
      return rejectWithValue(err.message);
    }
  }
);
