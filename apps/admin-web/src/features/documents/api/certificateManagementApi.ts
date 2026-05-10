import { documentsRoutes } from "./documentStorageApi";
export const certificateManagementApi = {
  list: documentsRoutes.certificates,
  issue: documentsRoutes.certificates,
  verify: documentsRoutes.certificateVerify,
};
