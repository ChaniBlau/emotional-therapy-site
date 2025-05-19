import { configureStore } from '@reduxjs/toolkit';
import userReducer from './userSlice';
import appointmentsReducer from './appointmentsSlice';

const store = configureStore({
  reducer: {
    user: userReducer,
    appointments: appointmentsReducer,
  },
});

export default store;