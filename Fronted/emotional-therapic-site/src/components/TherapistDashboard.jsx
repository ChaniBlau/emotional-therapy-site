import { useSelector } from "react-redux";


const TherapistDashboard = () => {
    const appointments = useSelector(state => state.appointments.appointments);
  return (
    <div>
      <h2>Your Appointments</h2>
      {appointments && appointments.length > 0 ? (
        appointments.map((app, index) => (
          <div key={index} className="appointment">
            <p><strong>Client Name:</strong> {app.name}</p>
            <p><strong>Date:</strong> {app.date ? new Date(app.date).toLocaleDateString() : ''}</p>
            <p><strong>Time:</strong> {app.date ? new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}</p>
            <p><strong>Email:</strong> {app.email}</p>
            <p><strong>Phone:</strong> {app.phoneNumber}</p>
            <p><strong>Age:</strong> {app.age}</p>
          </div>
        ))
      ) : (
        <p>No appointments found</p>
      )}
    </div>
  );
};

export default TherapistDashboard;