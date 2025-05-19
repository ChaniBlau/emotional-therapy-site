import { createSlice } from '@reduxjs/toolkit';

const userSlice = createSlice({
  name: 'user',
  initialState: {
    userInfo: null,  // יכיל את פרטי המשתמש
    token: null,     // טוקן לאימות
    role: null,      // "client" או "therapist"
    loading: false,
    error: null,
  },
  reducers: {
    setUser: (state, action) => {
      const { userInfo, token, role } = action.payload;
      state.userInfo = userInfo;
      state.token = token;
      state.role = role;
      state.error = null;
    },
    clearUser: (state) => {
      state.userInfo = null;
      state.token = null;
      state.role = null;
      state.error = null;
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
  setUser,
  clearUser,
  setLoading,
  setError,
} = userSlice.actions;

export default userSlice.reducer;