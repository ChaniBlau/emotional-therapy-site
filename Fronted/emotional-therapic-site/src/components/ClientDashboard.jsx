import { useSelector } from "react-redux";

function ClientDashboard() {
  const appointments = useSelector(state => state.appointments.appointments);

  return (
    <div>
      <h2>Your Appointments</h2>
      {appointments.length > 0 ? (
        appointments.map((app, index) => (
          <div key={index} className="appointment">
            <p><strong>Therapist Name:</strong> {app.name}</p>
            <p><strong>Date:</strong> {new Date(app.date).toLocaleDateString()}</p>
            <p><strong>Time:</strong> {new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
          </div>
        ))
      ) : (
        <p>No appointments found</p>
      )}
    </div>
    
  );
}
export default ClientDashboard;

