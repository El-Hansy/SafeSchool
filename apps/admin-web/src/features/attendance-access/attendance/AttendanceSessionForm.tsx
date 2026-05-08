export function AttendanceSessionForm() {
  return (
    <form aria-label="Attendance session">
      <label>
        Session name
        <input name="sessionName" />
      </label>
      <label>
        Attendance date
        <input name="attendanceDate" type="date" />
      </label>
      <button type="submit">Save</button>
    </form>
  );
}

export function GenerationActionPanel() {
  return <button type="button">Generate</button>;
}

