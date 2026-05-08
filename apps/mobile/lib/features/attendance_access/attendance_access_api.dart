class AttendanceAccessApi {
  AttendanceAccessApi({required this.schoolAccountId, this.authToken});

  final String schoolAccountId;
  final String? authToken;

  Map<String, String> get headers => {
        'content-type': 'application/json',
        'x-school-account-id': schoolAccountId,
        if (authToken != null) 'authorization': 'Bearer $authToken',
      };

  Uri path(String route) => Uri.parse('/api/v1/schools/$schoolAccountId/attendance-access$route');
}

