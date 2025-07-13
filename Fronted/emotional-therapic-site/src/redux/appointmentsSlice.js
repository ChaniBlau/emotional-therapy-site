
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
      state.success = null; // נקה הצלחה כאשר יש שגיאה
    },
    setSuccess: (state, action) => {
      state.success = action.payload;
      state.error = null; // נקה שגיאה כאשר יש הצלחה
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
      // הסר את התור מהרשימה לאחר ביטול מוצלח
      state.appointments = state.appointments.filter(
        (app) => app.id !== action.payload // בהנחה ש-action.payload הוא ה-ID של התור שבוטל
      );
      state.success = "Appointment cancelled successfully!"; // דוגמה לטיפול בהודעת הצלחה
    })
    .addCase(cancelAppointment.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload;
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
      .addCase(fetchAvailableTherapistsByDate.fulfilled, (state, action) => {
        state.availableTherapists = action.payload;
      })
      .addCase(scheduleAppointment.pending, (state) => {
        state.loading = true;
      })
      .addCase(scheduleAppointment.fulfilled, (state) => {
        state.loading = false;
      })
      .addCase(scheduleAppointment.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(fetchAvailableHours.fulfilled, (state, action) => {
        state.availableHours = action.payload;
      })
      .addCase(fetchAppointments.fulfilled, (state, action) => {
        state.appointments = action.payload;
      })
      .addCase(fetchTherapistAppointments.fulfilled, (state, action) => {
        state.appointments = action.payload;
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
