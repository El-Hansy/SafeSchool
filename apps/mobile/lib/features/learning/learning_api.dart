class LearningApiClient {
  LearningApiClient({required this.baseUrl, required this.tenantId, this.authToken, this.linkedStudentId});

  final String baseUrl;
  final String tenantId;
  final String? authToken;
  final String? linkedStudentId;

  Map<String, String> get headers => {
        'X-School-Account-Id': tenantId,
        if (authToken != null) 'Authorization': 'Bearer $authToken',
      };

  Uri schoolLearningUri(String path) => Uri.parse('$baseUrl/api/v1/schools/$tenantId/learning$path');
  Uri studentLearningUri(String path) => Uri.parse('$baseUrl/api/v1/students/me/learning$path');
  Uri guardianLearningUri(String studentProfileId, String path) => Uri.parse('$baseUrl/api/v1/guardians/me/students/$studentProfileId/learning$path');
}
