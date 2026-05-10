import { documentsRoutes } from "./documentStorageApi";
export const searchApi = { query: documentsRoutes.search, indexHealth: (schoolAccountId: string) => `${documentsRoutes.search(schoolAccountId)}/index-health` };
