import { createSlice } from '@reduxjs/toolkit';

const userSlice = createSlice({
  name: 'user',
  initialState: {
    userInfo: null,  
    token: null,     
    role: null,      
    loading: false,
    error: null,
  },
  reducers: {
   setUser: (state, action) => {
  state.userInfo = {
    id: action.payload.id,
    name: action.payload.name,
  };
  state.role = action.payload.role;
  state.token = action.payload.token || null;
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