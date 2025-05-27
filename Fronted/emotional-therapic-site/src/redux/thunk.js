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
      //console.log(data);
      const userFromServer = data[0];
      const isTherapist = data.length > 0 && userFromServer.Email !== undefined;
      dispatch(setUser({
        id: userFromServer.id,
        name: userFromServer.clientName,
        role: isTherapist ? "therapist" : "client"
      }));
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

export const scheduleAppointment = createAsyncThunk(
  'appointments/scheduleAppointment',
  async ({ therapistId, date, time, clientId }) => {
    const response = await fetch(`/api/ScheduleAppointment?therapistId=${therapistId}&date=${date}&time=${time}&clientId=${clientId}`, {
      method: 'POST',
    });
    if (!response.ok) {
      throw new Error('Failed to schedule appointment');
    }
    return await response.json();
  }
);

export const cancelAppointment = createAsyncThunk(
  'appointments/cancelAppointment',
  async ({ appointmentId, clientId }, { rejectWithValue }) => {
    try {
      const response = await fetch(`/api/Appointments/CancelAppointment?appointmentId=${appointmentId}&clientId=${clientId}`,
        { method: "DELETE" }
      );
      if (!response.ok)
        throw new Error("Failed to cancel appointment");

      return appointmentId;
    } catch (error) {
      return rejectWithValue(error.message);
    }
  }

);
