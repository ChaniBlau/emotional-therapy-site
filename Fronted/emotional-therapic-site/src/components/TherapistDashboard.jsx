import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react';
import { fetchTherapistAppointments } from '../redux/thunk';

const TherapistDashboard = () => {
  const dispatch = useDispatch();
  const userId = useSelector(state => state.user.userInfo?.id);
  const appointments = useSelector(state => state.appointments.appointments);

  useEffect(() => {
    if (userId) dispatch(fetchTherapistAppointments(userId));
  }, [dispatch, userId]);

  return (
    <div>
      {appointments && appointments.length > 0 ? (
        <>
          <h2>Your Appointments</h2>
          {appointments.map((app, index) => (
            <div key={index} className="appointment">
              <p><strong>Client Name:</strong> {app.name}</p>
              <p><strong>Date:</strong> {app.date ? new Date(app.date).toLocaleDateString() : ''}</p>
              <p><strong>Time:</strong> {app.date ? new Date(app.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}</p>
              <p><strong>Email:</strong> {app.email}</p>
              <p><strong>Phone:</strong> {app.phoneNumber}</p>
              <p><strong>Age:</strong> {app.age}</p>
            </div>
          ))}
        </>
      ) : (
        <p style={{ fontSize: "18px", marginTop: "1rem" }}>There are no regular patient appointments at the moment.</p>
      )}
    </div>
  );
};

export default TherapistDashboard;
