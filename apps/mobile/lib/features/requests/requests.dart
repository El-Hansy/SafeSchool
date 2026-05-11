import '../../core/api/mobile_api_client.dart';

class MobileRequestDraft {
  const MobileRequestDraft({
    required this.studentProfileId,
    required this.requestType,
    required this.reason,
    required this.requestedOutcome,
    required this.clientRequestId,
    this.startsAt,
    this.endsAt,
    this.submitterRole = 'guardian',
  });

  final String studentProfileId;
  final String requestType;
  final String reason;
  final String requestedOutcome;
  final String clientRequestId;
  final DateTime? startsAt;
  final DateTime? endsAt;
  final String submitterRole;

  bool get isValid =>
      studentProfileId.trim().isNotEmpty &&
      requestType.trim().isNotEmpty &&
      reason.trim().isNotEmpty &&
      requestedOutcome.trim().isNotEmpty &&
      clientRequestId.trim().isNotEmpty &&
      (startsAt == null || endsAt == null || startsAt!.isBefore(endsAt!));

  Map<String, dynamic> toJson() => {
        'studentProfileId': studentProfileId,
        'requestType': requestType,
        'reason': reason,
        'requestedOutcome': requestedOutcome,
        'clientRequestId': clientRequestId,
        'submitterRole': submitterRole,
        if (startsAt != null) 'startsAt': startsAt!.toUtc().toIso8601String(),
        if (endsAt != null) 'endsAt': endsAt!.toUtc().toIso8601String(),
      };
}

class MobileRequestResult {
  const MobileRequestResult({
    required this.requestId,
    required this.trackingReference,
    required this.requestType,
    required this.status,
    required this.priority,
    required this.studentProfileId,
    required this.visibleSummary,
    required this.auditTrail,
  });

  final String requestId;
  final String trackingReference;
  final String requestType;
  final String status;
  final String priority;
  final String studentProfileId;
  final String visibleSummary;
  final List<String> auditTrail;

  bool get accepted =>
      status == 'PendingApproval' ||
      status == 'NeedsReview' ||
      status == 'Approved';

  factory MobileRequestResult.demo(MobileRequestDraft draft) =>
      MobileRequestResult(
        requestId: 'demo-${draft.clientRequestId}',
        trackingReference: 'REQ-2026-0001',
        requestType: draft.requestType,
        status: 'PendingApproval',
        priority: draft.requestType.contains('early') ? 'High' : 'Normal',
        studentProfileId: draft.studentProfileId,
        visibleSummary: 'Request accepted and routed to approval workflow.',
        auditTrail: const ['submitted', 'approval-routing', 'demo-local'],
      );

  factory MobileRequestResult.fromJson(Map<String, dynamic> json) =>
      MobileRequestResult(
        requestId: json['requestId'] as String,
        trackingReference: json['trackingReference'] as String,
        requestType: json['requestType'] as String,
        status: json['status'] as String,
        priority: json['priority'] as String,
        studentProfileId: json['studentProfileId'] as String,
        visibleSummary: json['visibleSummary'] as String,
        auditTrail: _stringList(json['auditTrail']),
      );
}

class MobileRequestsRepository {
  const MobileRequestsRepository({this.apiClient = const MobileApiClient()});

  final MobileApiClient apiClient;

  Future<MobileRequestResult> submit(MobileRequestDraft draft) async {
    if (!draft.isValid) {
      throw ArgumentError.value(draft, 'draft', 'Invalid request draft.');
    }

    if (apiClient.usesDemoData && apiClient.jsonPost == null) {
      return MobileRequestResult.demo(draft);
    }

    final uri = switch (draft.submitterRole) {
      'student' => apiClient.studentUri('/requests'),
      'guardian' =>
        apiClient.guardianUri('/students/${draft.studentProfileId}/requests'),
      _ => apiClient.schoolUri('/requests'),
    };
    final payload = await apiClient.postJson(
      uri,
      draft.toJson(),
      actorPermissions: const ['requests.requests.create'],
    );
    return MobileRequestResult.fromJson(_mapFrom(payload));
  }

  Future<List<MobileRequestResult>> guardianHistory({
    String? studentProfileId,
  }) async {
    if (apiClient.usesDemoData && apiClient.jsonGet == null) {
      return [
        MobileRequestResult.demo(
          MobileRequestDraft(
            studentProfileId: studentProfileId ?? 'student-amina',
            requestType: 'early-leave',
            reason: 'Medical appointment',
            requestedOutcome: 'Release to guardian',
            clientRequestId: 'demo-history',
          ),
        ),
      ];
    }

    final uri = studentProfileId == null
        ? apiClient.guardianUri('/requests')
        : apiClient.guardianUri('/students/$studentProfileId/requests');
    final payload = await apiClient.getJson(
      uri,
      actorPermissions: const ['requests.guardian_history.read'],
    );
    return _listFrom(payload)
        .whereType<Map<String, dynamic>>()
        .map(MobileRequestResult.fromJson)
        .toList(growable: false);
  }
}

Map<String, dynamic> _mapFrom(Object? payload) {
  if (payload is Map<String, dynamic>) return payload;
  throw ArgumentError.value(payload, 'payload', 'Expected a JSON object.');
}

List<dynamic> _listFrom(Object? payload) {
  if (payload is List<dynamic>) return payload;
  if (payload is Map<String, dynamic> && payload['items'] is List<dynamic>) {
    return payload['items'] as List<dynamic>;
  }

  throw ArgumentError.value(payload, 'payload', 'Expected a JSON list.');
}

List<String> _stringList(Object? value) {
  if (value is List<dynamic>) {
    return value.map((item) => item.toString()).toList(growable: false);
  }

  return const [];
}
