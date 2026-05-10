import { documentsRoutes } from "./documentStorageApi";
export const searchApi = {
  query: documentsRoutes.search,
  open: documentsRoutes.searchOpen,
  indexHealth: documentsRoutes.searchIndexHealth,
};
