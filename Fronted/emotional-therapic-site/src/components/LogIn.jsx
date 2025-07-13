import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { loginUser } from '../redux/thunk';
import { setUser } from '../redux/userSlice';
import { useNavigate } from 'react-router-dom';
import {
  Paper, TextField, Button, Typography, Alert,
  Dialog, DialogTitle, DialogContent, DialogActions
} from '@mui/material';

const LogIn = () => {
  const [id, setId] = useState('');
  const [name, setName] = useState('');
  const [error, setError] = useState('');
  const [showDialog, setShowDialog] = useState(false);

  const dispatch = useDispatch();
  const navigate = useNavigate();
  const role = useSelector(state => state.user.role);

  useEffect(() => {
    if (role === "therapist") navigate('/therapist-dashboard');
    else if (role === "client") navigate('/client-dashboard');
  }, [role]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!id.trim() || !name.trim()) {
      setError("Please fill in all fields");
      return;
    }

    try {
      const resultAction = await dispatch(loginUser({ id, name }));

      if (loginUser.fulfilled.match(resultAction)) {
        dispatch(setUser({
          id,
          name,
          role: resultAction.payload[0]?.role?.toLowerCase() || null
        }));
      } else {
        setShowDialog(true);
      }
    } catch (err) {
      console.error("Login error:", err);
      setError(err.message || "Unexpected error");
    }
  };

  return (
    <Paper elevation={4} sx={{ p: 4, width: 350, borderRadius: 3 }}>
      <Typography variant="h4" align="center" color="primary" gutterBottom>
        Log in
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Id"
          variant="outlined"
          value={id}
          onChange={(e) => setId(e.target.value)}
          fullWidth
          margin="normal"
          required
          autoFocus
        />
        <TextField
          label="Name"
          variant="outlined"
          value={name}
          onChange={(e) => setName(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          sx={{ py: 1.3, fontWeight: "bold", fontSize: 17, borderRadius: 2 }}
          fullWidth
        >
          Log in
        </Button>
        {error && (
          <Alert severity="error" sx={{ mt: 2, fontWeight: "bold" }}>
            {error}
          </Alert>
        )}
      </form>

      <Dialog open={showDialog} onClose={() => setShowDialog(false)}>
        <DialogTitle>User not found</DialogTitle>
        <DialogContent>
          <Typography>
            The user does not exist in the system. Do you want to register?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowDialog(false)} color="secondary" variant="outlined">
            No
          </Button>
          <Button
            onClick={() => {
              setShowDialog(false);
              navigate('/signup');
            }}
            color="primary"
            variant="contained"
          >
            Yes, register now
          </Button>
        </DialogActions>
      </Dialog>
    </Paper>
  );
};

export default LogIn;
