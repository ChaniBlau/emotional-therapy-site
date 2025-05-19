import { createSlice } from '@reduxjs/toolkit';

const appointmentsSlice = createSlice({
  name: 'appointments',
  initialState: {
    appointments: [], // רשימת תורים פעילים
    loading: false,
    error: null,
  },
  reducers: {
    setAppointments: (state, action) => {
      state.appointments = action.payload;
      state.error = null;
    },
    clearAppointments: (state) => {
      state.appointments = [];
      state.error = null;
    },
    addAppointment: (state, action) => {
      state.appointments.push(action.payload);
    },
    cancelAppointment: (state, action) => {
      // פעולה שמוחקת תור לפי מזהה ייחודי
      const idToRemove = action.payload;
      state.appointments = state.appointments.filter(appt => appt.Id !== idToRemove);
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setError: (state, action) => {
      state.error = action.payload;
    },
  },
});

export const {
  setAppointments,
  clearAppointments,
  addAppointment,
  cancelAppointment,
  setLoading,
  setError,
} = appointmentsSlice.actions;

export default appointmentsSlice.reducer;
