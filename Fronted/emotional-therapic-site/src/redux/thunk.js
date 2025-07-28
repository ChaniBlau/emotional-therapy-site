import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

export const loginUser = createAsyncThunk(
  "user/loginUser",
  async ({ id, name }, thunkAPI) => {
    try {
      const response = await axios.get(
        `http://localhost:5222/api/Appointments/GetAllBusyAppointmentsForUser?id=${id}&name=${name}`
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const loginClient = createAsyncThunk(
  "user/loginClient",
  async ({ id, name }, thunkAPI) => {
    try {
      const res = await axios.get(`http://localhost:5222/api/Appointments/LoginClient`, {
        params: { id, name }
      });
      return res.data; // { id, name, role }
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || "Login failed");
    }
  }
);

export const loginTherapist = createAsyncThunk(
  "user/loginTherapist",
  async ({ id, name }, thunkAPI) => {
    try {
      const res = await axios.get(`http://localhost:5222/api/Appointments/LoginTherapist`, {
        params: { id, name }
      });
      return res.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || "Login failed");
    }
  }
);

export const signUpClient = createAsyncThunk(
  "client/signUpClient",
  async (clientData, thunkAPI) => {
    try {
      const response = await axios.post(
        `http://localhost:5222/api/Appointments/CreateNewClient`,
        clientData
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchTherapists = createAsyncThunk(
  "appointments/fetchTherapists",
  async (_, thunkAPI) => {
    try {
      const response = await axios.get("http://localhost:5222/api/Appointments/Therapists");
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchAvailableTherapistsByDate = createAsyncThunk(
  "appointments/fetchAvailableTherapistsByDate",
  async (date, thunkAPI) => {
    try {
      const response = await axios.get(
        `http://localhost:5222/api/Appointments/Client/AvailableTherapistsByDate?date=${date}`
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

// תיקון חשוב! זה היה קורא לScheduleAppointment במקום AvailableHours
export const fetchAvailableHours = createAsyncThunk(
  "appointments/fetchAvailableHours",
  async ({ therapistId, date }, thunkAPI) => {
    try {
      const response = await axios.get(
        `http://localhost:5222/api/Appointments/AvailableHours?therapistId=${therapistId}&date=${date}`
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const scheduleAppointment = createAsyncThunk(
  "appointments/scheduleAppointment",
  async ({ therapistId, date, time, clientId }, thunkAPI) => {
    try {
      const response = await axios.post(
        `http://localhost:5222/api/Appointments/ScheduleAppointment`,
        {
          therapistId: parseInt(therapistId.toString().trim()),
          date: date,
          time: time,
          clientId: parseInt(clientId.toString().trim())
        },
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const cancelAppointment = createAsyncThunk(
  "appointments/cancelAppointment",
  async ({ appointmentId, clientId }, thunkAPI) => {
    try {
      await axios.delete(`http://localhost:5222/api/Appointments/CancelAppointment?appointmentId=${appointmentId}&clientId=${clientId}`);
      return appointmentId;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchAppointments = createAsyncThunk(
  "appointments/fetchAppointments",
  async (clientId, thunkAPI) => {
    try {
      const name = thunkAPI.getState().user.userInfo?.name;
      const response = await axios.get(
        `http://localhost:5222/api/Appointments/GetAllBusyAppointmentsForUser`,
        {
          params: { id: clientId, name }
        }
      );
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchTherapistAppointments = createAsyncThunk(
  "appointments/fetchTherapistAppointments",
  async (therapistId, thunkAPI) => {
    try {
      const response = await axios.get("http://localhost:5222/api/Appointments/Therapist", {
        params: { therapistId }
      });
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);