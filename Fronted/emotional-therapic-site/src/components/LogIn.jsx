import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { loginUser } from '../redux/thunk';
import { useNavigate } from 'react-router-dom';

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
    <div className="login-container">
      <h2>Log in</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Id</label>
          <input type="text" value={id} onChange={(e) => setId(e.target.value)} />
        </div>
        <div>
          <label>Name</label>
          <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <button type="submit">Log in</button>
        {error && <p style={{ color: 'red' }}>{error}</p>}
      </form>
    </div>
  );
};

export default LogIn;
