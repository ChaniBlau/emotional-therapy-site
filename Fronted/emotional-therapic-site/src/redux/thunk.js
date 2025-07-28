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
      const cleanTherapistId = therapistId.toString().trim();
      const cleanClientId = clientId.toString().trim();
      
      console.log("Scheduling appointment with:", { 
        therapistId: cleanTherapistId, 
        date, 
        time, 
        clientId: cleanClientId 
      });
      
      const therapistCheck = await axios.get("http://localhost:5222/api/Appointments/Therapists");
      console.log("Available therapists:", therapistCheck.data.map(t => ({ id: t.id, name: t.firstName + " " + t.lastName })));
      console.log("Looking for therapist ID:", cleanTherapistId);
      
      const response = await axios.post(
        `http://localhost:5222/api/Appointments/ScheduleAppointment?therapistId=${cleanTherapistId}&date=${date}&time=${time}&clientId=${cleanClientId}`,
        {},
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );
      
      return response.data;
    } catch (error) {
      console.error("Schedule appointment error:", error.response?.data);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const cancelAppointment = createAsyncThunk(
  "appointments/cancelAppointment",
  async ({ appointmentId, clientId }, thunkAPI) => {
    try {
      console.log("Canceling appointment:", { appointmentId, clientId });
      
      await axios.delete(
        `http://localhost:5222/api/Appointments/CancelAppointment?appointmentId=${appointmentId}&clientId=${clientId}`,
        {
          timeout: 10000
        }
      );
      
      return appointmentId;
    } catch (error) {
      console.error("Cancel appointment error:", error);
      
      let errorMessage = "Failed to cancel appointment";
      
      if (error.response) {
        errorMessage = error.response.data?.message || 
                      error.response.data || 
                      `Server error: ${error.response.status}`;
      } else if (error.request) {
        errorMessage = "No response from server. Please check your connection.";
      } else {
        errorMessage = error.message || "Unknown error occurred";
      }

      return thunkAPI.rejectWithValue(errorMessage);
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
          params: { id: clientId, name },
          timeout: 10000
        }
      );
      return response.data;
    } catch (error) {
      console.error("Fetch appointments error:", error);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);

export const fetchTherapistAppointments = createAsyncThunk(
  "appointments/fetchTherapistAppointments",
  async (therapistId, thunkAPI) => {
    try {
      const response = await axios.get("http://localhost:5222/api/Appointments/Therapist", {
        params: { therapistId },
        timeout: 10000
      });
      return response.data;
    } catch (error) {
      console.error("Fetch therapist appointments error:", error);
      return thunkAPI.rejectWithValue(error.response?.data || error.message);
    }
  }
);