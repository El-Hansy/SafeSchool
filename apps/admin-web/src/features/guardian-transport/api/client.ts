export const guardianTransportApi = {
  base: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/transport`,
  plan: (studentProfileId: string) => `${guardianTransportApi.base(studentProfileId)}/plan`,
  progress: (studentProfileId: string, tripId: string) => `${guardianTransportApi.base(studentProfileId)}/trips/${tripId}/progress`,
  eta: (studentProfileId: string, tripId: string) => `${guardianTransportApi.base(studentProfileId)}/trips/${tripId}/eta`,
  notifications: (studentProfileId: string) => `${guardianTransportApi.base(studentProfileId)}/notifications`
};
