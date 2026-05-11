import '../../core/api/mobile_api_client.dart';

class MobileMedicalProfileDraft {
  const MobileMedicalProfileDraft({
    required this.studentProfileId,
    required this.summary,
    required this.restrictedDetail,
    required this.clientRequestId,
    this.severity = 'Routine',
  });

  final String studentProfileId;
  final String summary;
  final String restrictedDetail;
  final String clientRequestId;
  final String severity;

  bool get isValid =>
      studentProfileId.trim().isNotEmpty &&
      summary.trim().isNotEmpty &&
      restrictedDetail.trim().isNotEmpty &&
      clientRequestId.trim().isNotEmpty;

  Map<String, dynamic> toJson() => {
        'studentProfileId': studentProfileId,
        'summary': summary,
        'restrictedDetail': restrictedDetail,
        'clientRequestId': clientRequestId,
        'severity': severity,
      };
}

class MobileEmergencyAccessDraft {
  const MobileEmergencyAccessDraft({
    required this.studentProfileId,
    required this.reason,
    required this.actorId,
    required this.clientRequestId,
    this.actorRole = 'school-nurse',
    this.confirmed = true,
    this.breakGlass = false,
  });

  final String studentProfileId;
  final String reason;
  final String actorId;
  final String clientRequestId;
  final String actorRole;
  final bool confirmed;
  final bool breakGlass;

  bool get isValid =>
      studentProfileId.trim().isNotEmpty &&
      reason.trim().isNotEmpty &&
      actorId.trim().isNotEmpty &&
      clientRequestId.trim().isNotEmpty &&
      (!breakGlass || confirmed);

  Map<String, dynamic> toJson() => {
        'studentProfileId': studentProfileId,
        'reason': reason,
        'actorId': actorId,
        'clientRequestId': clientRequestId,
        'actorRole': actorRole,
        'confirmed': confirmed,
      };
}

class MobileMedicalIncidentDraft {
  const MobileMedicalIncidentDraft({
    required this.studentProfileId,
    required this.severity,
    required this.observation,
    required this.careAction,
    required this.clientRequestId,
  });

  final String studentProfileId;
  final String severity;
  final String observation;
  final String careAction;
  final String clientRequestId;

  bool get isValid =>
      studentProfileId.trim().isNotEmpty &&
      severity.trim().isNotEmpty &&
      observation.trim().isNotEmpty &&
      careAction.trim().isNotEmpty &&
      clientRequestId.trim().isNotEmpty;

  Map<String, dynamic> toJson() => {
        'studentProfileId': studentProfileId,
        'severity': severity,
        'observation': observation,
        'careAction': careAction,
        'clientRequestId': clientRequestId,
      };
}

class MobileMedicalResult {
  const MobileMedicalResult({
    required this.medicalRecordId,
    required this.recordReference,
    required this.studentProfileId,
    required this.recordType,
    required this.status,
    required this.severity,
    required this.visibleSummary,
    required this.auditTrail,
    this.expiresAt,
  });

  final String medicalRecordId;
  final String recordReference;
  final String studentProfileId;
  final String recordType;
  final String status;
  final String severity;
  final String visibleSummary;
  final List<String> auditTrail;
  final DateTime? expiresAt;

  factory MobileMedicalResult.demo({
    required String studentProfileId,
    required String recordType,
    required String clientRequestId,
  }) =>
      MobileMedicalResult(
        medicalRecordId: 'demo-$clientRequestId',
        recordReference: recordType == 'incident'
            ? 'INC-2026-0001'
            : recordType == 'emergency-access'
                ? 'EMG-2026-0001'
                : 'MED-2026-0001',
        studentProfileId: studentProfileId,
        recordType: recordType,
        status: recordType == 'emergency-access' ? 'Open' : 'Queued',
        severity: recordType == 'emergency-access' ? 'Urgent' : 'High',
        visibleSummary: 'Medical evidence captured with audit trail.',
        expiresAt: recordType == 'emergency-access'
            ? DateTime.now().toUtc().add(const Duration(minutes: 30))
            : null,
        auditTrail: const ['tenant-checked', 'feature-checked', 'demo-local'],
      );

  factory MobileMedicalResult.fromJson(Map<String, dynamic> json) =>
      MobileMedicalResult(
        medicalRecordId: json['medicalRecordId'] as String,
        recordReference: json['recordReference'] as String,
        studentProfileId: json['studentProfileId'] as String,
        recordType: json['recordType'] as String,
        status: json['status'] as String,
        severity: json['severity'] as String,
        visibleSummary: json['visibleSummary'] as String,
        expiresAt: json['expiresAt'] == null
            ? null
            : DateTime.parse(json['expiresAt'] as String),
        auditTrail: _stringList(json['auditTrail']),
      );
}

class MobileMedicalRepository {
  const MobileMedicalRepository({this.apiClient = const MobileApiClient()});

  final MobileApiClient apiClient;

  Future<MobileMedicalResult> submitGuardianUpdate(
    MobileMedicalProfileDraft draft,
  ) async {
    if (!draft.isValid) {
      throw ArgumentError.value(draft, 'draft', 'Invalid medical profile.');
    }

    if (apiClient.usesDemoData && apiClient.jsonPost == null) {
      return MobileMedicalResult.demo(
        studentProfileId: draft.studentProfileId,
        recordType: 'profile',
        clientRequestId: draft.clientRequestId,
      );
    }

    final payload = await apiClient.postJson(
      apiClient
          .guardianUri('/students/${draft.studentProfileId}/medical/updates'),
      draft.toJson(),
      actorPermissions: const ['medical.guardian_updates.submit'],
    );
    return MobileMedicalResult.fromJson(_mapFrom(payload));
  }

  Future<MobileMedicalResult> openEmergencyAccess(
    MobileEmergencyAccessDraft draft,
  ) async {
    if (!draft.isValid) {
      throw ArgumentError.value(draft, 'draft', 'Invalid emergency access.');
    }

    if (apiClient.usesDemoData && apiClient.jsonPost == null) {
      return MobileMedicalResult.demo(
        studentProfileId: draft.studentProfileId,
        recordType: 'emergency-access',
        clientRequestId: draft.clientRequestId,
      );
    }

    final path = draft.breakGlass
        ? '/medical/emergency/break-glass'
        : '/medical/emergency/access';
    final payload = await apiClient.postJson(
      apiClient.schoolUri(path),
      draft.toJson(),
      actorPermissions: [
        draft.breakGlass
            ? 'medical.emergency.break_glass'
            : 'medical.emergency.read',
      ],
    );
    return MobileMedicalResult.fromJson(_mapFrom(payload));
  }

  Future<MobileMedicalResult> logIncident(
    MobileMedicalIncidentDraft draft,
  ) async {
    if (!draft.isValid) {
      throw ArgumentError.value(draft, 'draft', 'Invalid medical incident.');
    }

    if (apiClient.usesDemoData && apiClient.jsonPost == null) {
      return MobileMedicalResult.demo(
        studentProfileId: draft.studentProfileId,
        recordType: 'incident',
        clientRequestId: draft.clientRequestId,
      );
    }

    final payload = await apiClient.postJson(
      apiClient.schoolUri('/medical/incidents'),
      draft.toJson(),
      actorPermissions: const ['medical.incidents.create'],
    );
    return MobileMedicalResult.fromJson(_mapFrom(payload));
  }
}

Map<String, dynamic> _mapFrom(Object? payload) {
  if (payload is Map<String, dynamic>) return payload;
  throw ArgumentError.value(payload, 'payload', 'Expected a JSON object.');
}

List<String> _stringList(Object? value) {
  if (value is List<dynamic>) {
    return value.map((item) => item.toString()).toList(growable: false);
  }

  return const [];
}
