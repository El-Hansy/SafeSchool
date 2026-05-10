export const documentsRoutes = {
  documents: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/documents`,
  certificates: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/certificates`,
  search: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/search`,
};
export const documentStorageApi = documentsRoutes;
