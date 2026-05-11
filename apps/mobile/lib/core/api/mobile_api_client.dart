import 'dart:convert';
import 'dart:io';

typedef MobileJsonGet = Future<Object?> Function(
  Uri uri,
  Map<String, String> headers,
);
typedef MobileJsonPost = Future<Object?> Function(
  Uri uri,
  Map<String, String> headers,
  Map<String, dynamic> body,
);

class MobileApiClient {
  const MobileApiClient({
    this.baseUrl = '',
    this.schoolAccountId = 'school-demo',
    this.authToken,
    this.jsonGet,
    this.jsonPost,
  });

  factory MobileApiClient.fromEnvironment() {
    const baseUrl = String.fromEnvironment('SAFE_SCHOOL_API_BASE_URL');
    const schoolAccountId = String.fromEnvironment(
      'SAFE_SCHOOL_TENANT_ID',
      defaultValue: 'school-demo',
    );
    const authToken = String.fromEnvironment('SAFE_SCHOOL_AUTH_TOKEN');

    return MobileApiClient(
      baseUrl: baseUrl,
      schoolAccountId: schoolAccountId,
      authToken: authToken.isEmpty ? null : authToken,
    );
  }

  final String baseUrl;
  final String schoolAccountId;
  final String? authToken;
  final MobileJsonGet? jsonGet;
  final MobileJsonPost? jsonPost;

  bool get isConfigured => baseUrl.trim().isNotEmpty;
  bool get usesDemoData => !isConfigured;

  List<MobileRoleWorkspace> fetchWorkspaces() => MobileRoleWorkspace.demo;

  Uri apiUri(String path) {
    final normalizedPath = path.startsWith('/') ? path : '/$path';

    if (!isConfigured) {
      return Uri.parse(normalizedPath);
    }

    final normalizedBaseUrl = baseUrl.endsWith('/')
        ? baseUrl.substring(0, baseUrl.length - 1)
        : baseUrl;
    return Uri.parse('$normalizedBaseUrl$normalizedPath');
  }

  Uri schoolUri(String path) => apiUri('/api/v1/schools/$schoolAccountId$path');
  Uri guardianUri(String path) => apiUri('/api/v1/guardians/me$path');
  Uri studentUri(String path) => apiUri('/api/v1/students/me$path');
  Uri mobileUri(String path, [Map<String, String?> query = const {}]) {
    final cleanPath = path.startsWith('/') ? path : '/$path';
    final uri = apiUri('/api/v1/mobile$cleanPath');
    final cleanQuery = {
      for (final entry in query.entries)
        if (entry.value != null && entry.value!.isNotEmpty)
          entry.key: entry.value!,
    };

    return cleanQuery.isEmpty
        ? uri
        : uri.replace(queryParameters: {
            ...uri.queryParameters,
            ...cleanQuery,
          });
  }

  Map<String, String> headers({
    String? idempotencyKey,
    Iterable<String> actorPermissions = const [],
  }) =>
      {
        'X-School-Account-Id': schoolAccountId,
        if (authToken?.isNotEmpty == true) 'Authorization': 'Bearer $authToken',
        if (idempotencyKey?.isNotEmpty == true)
          'Idempotency-Key': idempotencyKey!,
        if (actorPermissions.isNotEmpty)
          'X-Actor-Permissions': actorPermissions.join(' '),
      };

  MobileRelease currentRelease({int versionCode = 1200}) {
    return MobileRelease(
      id: 'release-12',
      versionName: '12.0.0',
      versionCode: 1200,
      updateRequired: versionCode < 1200,
      checksum: 'sha256-demo-phase12',
    );
  }

  Future<MobileProfile> fetchProfile({
    String roleCode = 'guardian',
    String languageCode = 'en',
  }) async {
    if (usesDemoData && jsonGet == null) {
      return MobileProfile.demo(
        tenantId: schoolAccountId,
        roleCode: roleCode,
        languageCode: languageCode,
      );
    }

    final payload = await getJson(mobileUri('/profile', {
      'tenantId': schoolAccountId,
      'roleCode': roleCode,
      'languageCode': languageCode,
    }));
    return MobileProfile.fromJson(_mapFrom(payload));
  }

  Future<List<MobileRoleWorkspace>> fetchLiveWorkspaces({
    String roleCode = 'guardian',
    String languageCode = 'en',
  }) async {
    if (usesDemoData && jsonGet == null) {
      return fetchWorkspaces();
    }

    final payload = await getJson(mobileUri('/workspaces', {
      'tenantId': schoolAccountId,
      'roleCode': roleCode,
      'languageCode': languageCode,
    }));
    final rows = payload is Map<String, dynamic>
        ? _listFrom(payload['items'])
        : _listFrom(payload);
    return rows
        .whereType<Map<String, dynamic>>()
        .map(MobileRoleWorkspace.fromApiJson)
        .toList(growable: false);
  }

  Future<MobileContext> selectContext({
    required String roleCode,
    required String languageCode,
    required String deviceId,
  }) async {
    if (usesDemoData && jsonPost == null) {
      return MobileContext(
        activeTenantId: schoolAccountId,
        activeRoleCode: roleCode,
        languageCode: languageCode,
        textDirection: languageCode == 'ar' ? 'rtl' : 'ltr',
        workspace: MobileRoleWorkspace.demo.firstWhere(
          (workspace) => workspace.roleCode == roleCode,
          orElse: () => MobileRoleWorkspace.demo.first,
        ),
      );
    }

    final payload = await postJson(mobileUri('/context'), {
      'tenantId': schoolAccountId,
      'roleCode': roleCode,
      'languageCode': languageCode,
      'deviceId': deviceId,
    });
    return MobileContext.fromJson(_mapFrom(payload));
  }

  Future<MobileRelease> fetchCurrentRelease({
    int versionCode = 1200,
    String roleCode = 'guardian',
    String? deviceId,
  }) async {
    if (usesDemoData && jsonGet == null) {
      return currentRelease(versionCode: versionCode);
    }

    final payload = await getJson(mobileUri('/releases/current', {
      'tenantId': schoolAccountId,
      'roleCode': roleCode,
      'deviceId': deviceId,
      'versionCode': versionCode.toString(),
    }));
    return MobileRelease.fromJson(_mapFrom(payload));
  }

  Future<InstallEventResult> recordInstallEvent({
    required String deviceId,
    required String releaseId,
    required String versionName,
    required int versionCode,
    String userId = 'anonymous',
    String eventType = 'Launch',
    String eventResult = 'Allowed',
    DateTime? occurredAt,
  }) async {
    if (usesDemoData && jsonPost == null) {
      return const InstallEventResult(
        eventId: 'demo-install-event',
        accepted: true,
        nextAction: 'continue',
        userMessage: 'Version accepted.',
      );
    }

    final payload = await postJson(mobileUri('/install-events'), {
      'deviceId': deviceId,
      'tenantId': schoolAccountId,
      'userId': userId,
      'releaseId': releaseId,
      'versionName': versionName,
      'versionCode': versionCode,
      'eventType': eventType,
      'eventResult': eventResult,
      'occurredAt': (occurredAt ?? DateTime.now()).toUtc().toIso8601String(),
    });
    return InstallEventResult.fromJson(_mapFrom(payload));
  }

  Future<Object?> getJson(
    Uri uri, {
    Iterable<String> actorPermissions = const [],
  }) async {
    final handler = jsonGet ?? _defaultGetJson;
    return handler(uri, headers(actorPermissions: actorPermissions));
  }

  Future<Object?> postJson(
    Uri uri,
    Map<String, dynamic> body, {
    Iterable<String> actorPermissions = const [],
  }) async {
    final handler = jsonPost ?? _defaultPostJson;
    final idempotencyKey =
        body['clientActionId'] as String? ?? body['clientRequestId'] as String?;
    return handler(
      uri,
      headers(
        idempotencyKey: idempotencyKey,
        actorPermissions: actorPermissions,
      ),
      body,
    );
  }

  static Future<Object?> _defaultGetJson(
    Uri uri,
    Map<String, String> headers,
  ) async {
    final client = HttpClient();
    try {
      final request = await client.getUrl(uri);
      headers.forEach(request.headers.set);
      final response = await request.close();
      return _decodeResponse(uri, response);
    } finally {
      client.close(force: true);
    }
  }

  static Future<Object?> _defaultPostJson(
    Uri uri,
    Map<String, String> headers,
    Map<String, dynamic> body,
  ) async {
    final client = HttpClient();
    try {
      final request = await client.postUrl(uri);
      headers.forEach(request.headers.set);
      request.headers.contentType = ContentType.json;
      request.write(jsonEncode(body));
      final response = await request.close();
      return _decodeResponse(uri, response);
    } finally {
      client.close(force: true);
    }
  }

  static Future<Object?> _decodeResponse(
    Uri uri,
    HttpClientResponse response,
  ) async {
    final body = await utf8.decodeStream(response);
    if (response.statusCode < 200 || response.statusCode >= 300) {
      throw MobileApiException(uri, response.statusCode, body);
    }

    if (body.trim().isEmpty) {
      return <String, dynamic>{};
    }

    final decoded = jsonDecode(body);
    if (decoded is Map<String, dynamic> || decoded is List<dynamic>) {
      return decoded;
    }

    throw MobileApiException(
      uri,
      response.statusCode,
      'Expected JSON object or array.',
    );
  }
}

class MobileApiException implements Exception {
  const MobileApiException(this.uri, this.statusCode, this.body);

  final Uri uri;
  final int statusCode;
  final String body;

  @override
  String toString() => 'MobileApiException($statusCode $uri): $body';
}

class MobileProfile {
  const MobileProfile({
    required this.userId,
    required this.activeTenantId,
    required this.availableTenants,
    required this.availableRoles,
    required this.linkedStudents,
    required this.languageCode,
    required this.textDirection,
    required this.mobileAccessStatus,
    this.deniedReason,
  });

  final String userId;
  final String activeTenantId;
  final List<String> availableTenants;
  final List<String> availableRoles;
  final List<String> linkedStudents;
  final String languageCode;
  final String textDirection;
  final String mobileAccessStatus;
  final String? deniedReason;

  factory MobileProfile.demo({
    required String tenantId,
    required String roleCode,
    required String languageCode,
  }) =>
      MobileProfile(
        userId: '$roleCode-demo',
        activeTenantId: tenantId,
        availableTenants: const ['school-demo', 'school-pilot'],
        availableRoles: MobileRoleWorkspace.demo
            .map((workspace) => workspace.roleCode)
            .toList(),
        linkedStudents: roleCode == 'guardian'
            ? const ['student-amina', 'student-omar']
            : roleCode == 'student'
                ? const ['student-self']
                : const [],
        languageCode: languageCode == 'ar' ? 'ar' : 'en',
        textDirection: languageCode == 'ar' ? 'rtl' : 'ltr',
        mobileAccessStatus: 'active',
      );

  factory MobileProfile.fromJson(Map<String, dynamic> json) => MobileProfile(
        userId: json['userId'] as String,
        activeTenantId: json['activeTenantId'] as String,
        availableTenants: _stringList(json['availableTenants']),
        availableRoles: _stringList(json['availableRoles']),
        linkedStudents: _stringList(json['linkedStudents']),
        languageCode: json['languageCode'] as String,
        textDirection: json['textDirection'] as String,
        mobileAccessStatus: json['mobileAccessStatus'] as String,
        deniedReason: json['deniedReason'] as String?,
      );
}

class MobileContext {
  const MobileContext({
    required this.activeTenantId,
    required this.activeRoleCode,
    required this.languageCode,
    required this.textDirection,
    required this.workspace,
  });

  final String activeTenantId;
  final String activeRoleCode;
  final String languageCode;
  final String textDirection;
  final MobileRoleWorkspace workspace;

  factory MobileContext.fromJson(Map<String, dynamic> json) => MobileContext(
        activeTenantId: json['activeTenantId'] as String,
        activeRoleCode: json['activeRoleCode'] as String,
        languageCode: json['languageCode'] as String,
        textDirection: json['textDirection'] as String,
        workspace: MobileRoleWorkspace.fromApiJson(
          json['workspaceSummary'] as Map<String, dynamic>,
        ),
      );
}

class InstallEventResult {
  const InstallEventResult({
    required this.eventId,
    required this.accepted,
    required this.nextAction,
    required this.userMessage,
  });

  final String eventId;
  final bool accepted;
  final String nextAction;
  final String userMessage;

  factory InstallEventResult.fromJson(Map<String, dynamic> json) =>
      InstallEventResult(
        eventId: json['eventId'] as String,
        accepted: json['accepted'] as bool,
        nextAction: json['nextAction'] as String,
        userMessage: json['userMessage'] as String,
      );
}

class MobileRoleWorkspace {
  const MobileRoleWorkspace({
    required this.roleCode,
    required this.label,
    required this.arabicLabel,
    required this.actions,
    required this.offlineAllowed,
  });

  final String roleCode;
  final String label;
  final String arabicLabel;
  final List<String> actions;
  final bool offlineAllowed;

  factory MobileRoleWorkspace.fromApiJson(Map<String, dynamic> json) {
    final actions = _listFrom(json['actions'])
        .map((item) => item is Map<String, dynamic>
            ? (item['actionCode'] ?? item['sourceFeatureCode']).toString()
            : item.toString())
        .toList(growable: false);
    final offlineCapabilities = _stringList(json['offlineCapabilities']);

    return MobileRoleWorkspace(
      roleCode: (json['roleCode'] ?? json['workspaceCode']).toString(),
      label: (json['displayName'] ?? json['workspaceCode']).toString(),
      arabicLabel: (json['displayName'] ?? json['workspaceCode']).toString(),
      actions: actions,
      offlineAllowed: offlineCapabilities.isNotEmpty,
    );
  }

  static const demo = <MobileRoleWorkspace>[
    MobileRoleWorkspace(
        roleCode: 'guardian',
        label: 'Guardian',
        arabicLabel: 'ولي الأمر',
        actions: ['View students', 'Submit request', 'Submit complaint'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'student',
        label: 'Student',
        arabicLabel: 'الطالب',
        actions: ['Learning', 'Messages', 'Documents'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'transport_driver',
        label: 'Transport driver',
        arabicLabel: 'سائق الحافلة',
        actions: ['Trip', 'Boarding', 'Drop'],
        offlineAllowed: true),
    MobileRoleWorkspace(
        roleCode: 'gate_access',
        label: 'Gate/access staff',
        arabicLabel: 'بوابة المدرسة',
        actions: ['NFC scan', 'QR scan'],
        offlineAllowed: true),
    MobileRoleWorkspace(
        roleCode: 'canteen_cashier',
        label: 'Canteen cashier',
        arabicLabel: 'المقصف',
        actions: ['Wallet scan', 'Charge'],
        offlineAllowed: true),
    MobileRoleWorkspace(
        roleCode: 'teacher',
        label: 'Teacher',
        arabicLabel: 'المعلم',
        actions: ['Class', 'Behavior'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'medical_staff',
        label: 'Medical staff',
        arabicLabel: 'العيادة',
        actions: ['Medical profile', 'Emergency'],
        offlineAllowed: true),
    MobileRoleWorkspace(
        roleCode: 'complaint_handler',
        label: 'Complaint handler',
        arabicLabel: 'الشكاوى',
        actions: ['Triage', 'Escalate'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'communication_sender',
        label: 'Communication sender',
        arabicLabel: 'الرسائل',
        actions: ['Broadcast', 'Direct message'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'document_administrator',
        label: 'Document administrator',
        arabicLabel: 'الوثائق',
        actions: ['Documents', 'Certificates'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'school_administrator',
        label: 'School administrator',
        arabicLabel: 'إدارة المدرسة',
        actions: ['Roles', 'Release'],
        offlineAllowed: false),
    MobileRoleWorkspace(
        roleCode: 'platform_support',
        label: 'Platform support',
        arabicLabel: 'الدعم',
        actions: ['Diagnostics', 'Audit'],
        offlineAllowed: false),
  ];
}

class MobileRelease {
  const MobileRelease({
    required this.id,
    required this.versionName,
    required this.versionCode,
    required this.updateRequired,
    required this.checksum,
  });

  final String id;
  final String versionName;
  final int versionCode;
  final bool updateRequired;
  final String checksum;

  factory MobileRelease.fromJson(Map<String, dynamic> json) => MobileRelease(
        id: json['releaseId'] as String,
        versionName: json['versionName'] as String,
        versionCode: json['versionCode'] as int,
        updateRequired: json['updateRequired'] as bool,
        checksum: json['checksum'] as String,
      );
}

List<dynamic> _listFrom(Object? value) {
  if (value is List) {
    return value;
  }
  return const [];
}

Map<String, dynamic> _mapFrom(Object? value) {
  if (value is Map<String, dynamic>) {
    return value;
  }

  throw const FormatException('Expected JSON object.');
}

List<String> _stringList(Object? value) =>
    _listFrom(value).map((item) => item.toString()).toList(growable: false);
