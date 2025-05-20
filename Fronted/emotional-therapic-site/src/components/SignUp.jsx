import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { signUpClient } from "../redux/thunk";

const SignUp = () => {
  const dispatch = useDispatch();
  const { loading, error } = useSelector((state) => state.client);

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    phoneNumber: "",
    yearOfBirth: "",
    email: ""
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    dispatch(signUpClient({ ...form, yearOfBirth: parseInt(form.yearOfBirth) }));
  };

  return (
    <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "auto" }}>
      <h1>Sign up</h1>
      <input name="firstName" placeholder="First Name" value={form.firstName} onChange={handleChange} required />
      <input name="lastName" placeholder="Last Name" value={form.lastName} onChange={handleChange} required />
      <input name="phoneNumber" placeholder="Phone Number" value={form.phoneNumber} onChange={handleChange} required />
      <input name="yearOfBirth" placeholder="Year of Birth" type="number" min="1900" max={new Date().getFullYear()} value={form.yearOfBirth} onChange={handleChange} required />
      <input name="email" placeholder="Email" type="email" value={form.email} onChange={handleChange} required />
      <button type="submit" disabled={loading} style={{ marginTop: 12 }}>
        {loading ? "Signing Up..." : "Sign Up"}
      </button>
      {error && <div style={{ color: "red" }}>{error}</div>}
    </form>
  );
};

export default SignUp;