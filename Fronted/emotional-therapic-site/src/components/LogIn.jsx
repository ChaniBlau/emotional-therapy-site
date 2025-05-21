// import React, { useState } from 'react';
// import { useDispatch } from 'react-redux';
// import { loginUser } from '../redux/thunk';
// import { useNavigate } from 'react-router-dom';

// const LogIn = () => {
//   const [id, setId] = useState('');
//   const [name, setName] = useState('');
//   const [error, setError] = useState('');

//   const dispatch = useDispatch();
//   const navigate = useNavigate();

//   const handleSubmit = async (e) => {
//     e.preventDefault();

//     if (!id.trim() || !name.trim()) {
//       setError("Please fill in all fields");
//       return;
//     }

//     try {
//       const resultAction = await dispatch(loginUser({ id, name }));

//       if (loginUser.fulfilled.match(resultAction)) {
//         const user = resultAction.payload[0];
//         const isTherapist = user.role === "Therapist";
//         navigate(isTherapist ? '/therapist-dashboard' : '/client-dashboard');
//       } else {
//         setError(resultAction.payload || "error in login");
//       }
//     } catch (err) {
//       setError("Unexpected error");
//     }
//   };

//   return (
//     <div className="login-container">
//       <h2>Log in</h2>
//       <form onSubmit={handleSubmit}>
//         <div>
//           <label>Id</label>
//           <input type="text" value={id} onChange={(e) => setId(e.target.value)} />
//         </div>
//         <div>
//           <label>Name</label>
//           <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
//         </div>
//         <button type="submit">Log in</button>
//         {error && <p style={{ color: 'red' }}>{error}</p>}
//       </form>
//     </div>
//   );
// };

// export default LogIn;

import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { loginUser } from '../redux/thunk';
import { useNavigate } from 'react-router-dom';
import {
Box,
Paper,
TextField,
Button,
Typography,
Alert,
} from '@mui/material';

const LogIn = () => {
const [id, setId] = useState('');
const [name, setName] = useState('');
const [error, setError] = useState('');

const dispatch = useDispatch();
const navigate = useNavigate();

const handleSubmit = async (e) => {
e.preventDefault();

if (!id.trim() || !name.trim()) {
  setError("Please fill in all fields");
  return;
}

try {
  const resultAction = await dispatch(loginUser({ id, name }));

  if (loginUser.fulfilled.match(resultAction)) {
    const user = resultAction.payload[0];
    const isTherapist = user.role === "Therapist";
    navigate(isTherapist ? '/therapist-dashboard' : '/client-dashboard');
  } else {
    setError(resultAction.payload || "error in login");
  }
} catch (err) {
  setError("Unexpected error");
}
};

return (
<Box
className="d-flex justify-content-center align-items-center"
style={{ minHeight: '100vh', background: '#f2f6fc' }}
>
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
className="w-100 mt-3"
sx={{ py: 1.3, fontWeight: "bold", fontSize: 17, borderRadius: 2 }}
>
Log in
</Button>
{error && (
<Alert severity="error" sx={{ mt: 2, fontWeight: "bold" }}>
{error}
</Alert>
)}
</form>
</Paper>
</Box>
);
};

export default LogIn;