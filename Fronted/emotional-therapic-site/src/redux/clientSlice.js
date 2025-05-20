import { createSlice } from '@reduxjs/toolkit';

const clientSlice = createSlice({
    name: 'client',
    initialState: {
        client: null, 
        loading: false,
        error: null,
    },
    reducers: {
        setClient: (state, action) => {
        state.client = action.payload;
        state.error = null;
        },
        clearClient: (state) => {
        state.client = null;
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
    export default clientSlice.reducer;
    export const { setClient, clearClient, setLoading, setError } = clientSlice.actions;