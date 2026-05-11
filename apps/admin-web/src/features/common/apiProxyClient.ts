export async function postSafeSchoolJson(path: string, schoolAccountId: string, actorReference: string, body: object) {
  return fetch("/api/safeschool/proxy", {
    method: "POST",
    headers: {
      "content-type": "application/json",
    },
    body: JSON.stringify({
      path,
      schoolAccountId,
      actorReference,
      body,
    }),
  });
}

