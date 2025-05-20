import { configureStore } from '@reduxjs/toolkit';
import userReducer from './userSlice';
import appointmentsReducer from './appointmentsSlice';
import clientReducer from './clientSlice';

const store = configureStore({
  reducer: {
    user: userReducer,
    appointments: appointmentsReducer,
    client: clientReducer,
  },
});

export default store;