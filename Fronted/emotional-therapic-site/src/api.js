export const getBusyAppointmentsForUser = async (id, name) => {
  if (!id || !name) {
    throw new Error("Missing user ID or name");
  }
  const res = await fetch(`http://localhost:5222/api/Appointments/GetAllBusyAppointmentsForUser?id=${id}&name=${name}`);
  if (!res.ok) {
    throw new Error("Failed to fetch busy appointments");
  }
  return await res.json();
};