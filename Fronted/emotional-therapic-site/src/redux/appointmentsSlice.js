import { createSlice } from '@reduxjs/toolkit';
import { scheduleAppointment, cancelAppointment } from './thunk';

const appointmentsSlice = createSlice({
  name: 'appointments',
  initialState: {
    appointments: [],
    loading: false,
    error: null,
    status: 'idle', // הוסף שדה status
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
      const idToRemove = action.payload;
      state.appointments = state.appointments.filter(
        appt => appt.Id !== idToRemove && appt.Code !== idToRemove
      );
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setError: (state, action) => {
      state.error = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      // קביעת תור
      .addCase(scheduleAppointment.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(scheduleAppointment.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.appointments.push(action.payload);
      })
      .addCase(scheduleAppointment.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      })
      // ביטול תור
      .addCase(cancelAppointment.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(cancelAppointment.fulfilled, (state, action) => {
        state.status = 'succeeded';
        const idToRemove = action.payload;
        state.appointments = state.appointments.filter(
          appt => appt.id !== idToRemove && appt.code !== idToRemove
        );
      })
      .addCase(cancelAppointment.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error ? action.error.message : 'Failed to cancel appointment';
      });
  }
});

export const {
  setAppointments,
  clearAppointments,
  addAppointment,
  cancelAppointment: cancelAppointmentAction,
  setLoading,
  setError,
} = appointmentsSlice.actions;

export default appointmentsSlice.reducer;