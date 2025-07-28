import { createSlice } from "@reduxjs/toolkit";
import {
  fetchTherapists,
  fetchAvailableTherapistsByDate,
  scheduleAppointment,
  fetchAvailableHours,
  fetchAppointments,
  fetchTherapistAppointments,
  cancelAppointment
} from "./thunk";

const appointmentsSlice = createSlice({
  name: "appointments",
  initialState: {
    therapists: [],
    availableTherapists: [],
    appointments: [],
    selectedTherapist: "",
    selectedDate: "",
    selectedTime: "",
    mode: "therapist",
    loading: false,
    error: null,
    success: null,
    availableHours: []
  },
  reducers: {
    setSelectedTherapist: (state, action) => {
      state.selectedTherapist = action.payload;
    },
    setSelectedDate: (state, action) => {
      state.selectedDate = action.payload;
    },
    setSelectedTime: (state, action) => {
      state.selectedTime = action.payload;
    },
    setMode: (state, action) => {
      state.mode = action.payload;
    },
    clearStatus: (state) => {
      state.error = null;
      state.success = null;
    },
    setError: (state, action) => {
      state.error = action.payload;
      state.success = null;
    },
    setSuccess: (state, action) => {
      state.success = action.payload;
      state.error = null;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(cancelAppointment.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(cancelAppointment.fulfilled, (state, action) => {
        state.loading = false;
        const appointmentIdToRemove = action.payload;

        state.appointments = state.appointments.filter((app) => {
          const appId = app.appointmentId || app.id || app.code || app.Code;
          return appId != appointmentIdToRemove;
        });

        state.success = "Appointment cancelled successfully!";
      })
      .addCase(cancelAppointment.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to cancel appointment";
      })
      .addCase(fetchTherapists.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchTherapists.fulfilled, (state, action) => {
        state.therapists = action.payload;
        state.loading = false;
      })
      .addCase(fetchTherapists.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })

      .addCase(fetchAvailableTherapistsByDate.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAvailableTherapistsByDate.fulfilled, (state, action) => {
        state.availableTherapists = action.payload;
        state.loading = false;
      })
      .addCase(fetchAvailableTherapistsByDate.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(scheduleAppointment.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.success = null;
      })
      .addCase(scheduleAppointment.fulfilled, (state, action) => {
        state.loading = false;
        state.success = action.payload?.message || "Appointment scheduled successfully!";
        state.error = null;
        state.selectedTherapist = "";
        state.selectedDate = "";
        state.selectedTime = "";
        state.availableHours = [];
      })
      .addCase(scheduleAppointment.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to schedule appointment";
        state.success = null;
      })
      .addCase(fetchAvailableHours.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAvailableHours.fulfilled, (state, action) => {
        state.availableHours = action.payload || [];
        state.loading = false;
      })
      .addCase(fetchAvailableHours.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
        state.availableHours = []; 
      })

      .addCase(fetchAppointments.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAppointments.fulfilled, (state, action) => {
        state.appointments = action.payload;
        state.loading = false;
      })
      .addCase(fetchAppointments.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })

      .addCase(fetchTherapistAppointments.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchTherapistAppointments.fulfilled, (state, action) => {
        state.appointments = action.payload;
        state.loading = false;
      })
      .addCase(fetchTherapistAppointments.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      });
  }
});

export const {
  setSelectedTherapist,
  setSelectedDate,
  setSelectedTime,
  setMode,
  clearStatus,
  setError,
  setSuccess
} = appointmentsSlice.actions;

export default appointmentsSlice.reducer;