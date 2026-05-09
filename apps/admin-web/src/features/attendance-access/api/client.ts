export type AttendanceAccessError = {
  code: string;
  message: string;
  target?: string;
};

export type AttendanceAccessClientOptions = {
  schoolAccountId: string;
  token?: string;
};

export function attendanceAccessPath(schoolAccountId: string, path: string) {
  return `/api/v1/schools/${schoolAccountId}/attendance-access${path}`;
}

export function attendanceAccessHeaders(options: AttendanceAccessClientOptions) {
  return {
    "content-type": "application/json",
    "x-school-account-id": options.schoolAccountId,
    ...(options.token ? { authorization: `Bearer ${options.token}` } : {}),
  };
}

