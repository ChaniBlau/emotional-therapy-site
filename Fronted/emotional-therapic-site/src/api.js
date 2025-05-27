export async function getBusyAppointmentsForUser(id, name) {
  const res = await fetch(`/api/Appointments/GetAllBusyAppointmentsForUser?id=${id}&name=${name}`);
  if (!res.ok) throw new Error("Failed to fetch busy appointments");
  return await res.json();
}

export async function createNewClient(clientData) {
  const res = await fetch('/api/Appointments/CreateNewClient', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(clientData),
  });
  if (!res.ok) throw new Error("Registration failed");
  return await res.json();
}

export async function scheduleAppointment({ therapistId, date, time, clientId }) {
  const res = await fetch(`/api/ScheduleAppointment?therapistId=${therapistId}&date=${date}&time=${time}&clientId=${clientId}`, {
    method: 'POST',
  });
  if (!res.ok) throw new Error('Failed to schedule appointment');
  return await res.json();
}

export async function cancelAppointment({ appointmentId, clientId }) {
  const res = await fetch(`/api/Appointments/CancelAppointment?appointmentId=${appointmentId}&clientId=${clientId}`, {
    method: "DELETE"
  });
  if (!res.ok) throw new Error("Failed to cancel appointment");
  return appointmentId;
}