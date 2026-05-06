class CredentialStatusSnapshot {
  const CredentialStatusSnapshot({
    required this.credentialStatusSnapshotId,
    required this.schoolAccountId,
    required this.studentProfileId,
    required this.identityCredentialId,
    required this.credentialType,
    required this.credentialStatus,
    required this.validFrom,
    required this.snapshotGeneratedAt,
    required this.snapshotExpiresAt,
    this.validUntil,
    this.revokedAt,
  });

  final String credentialStatusSnapshotId;
  final String schoolAccountId;
  final String studentProfileId;
  final String identityCredentialId;
  final String credentialType;
  final String credentialStatus;
  final DateTime validFrom;
  final DateTime? validUntil;
  final DateTime? revokedAt;
  final DateTime snapshotGeneratedAt;
  final DateTime snapshotExpiresAt;

  bool get isUsableIdentityEvidence =>
      credentialStatus == 'Active' &&
      revokedAt == null &&
      snapshotExpiresAt.isAfter(DateTime.now().toUtc());

  factory CredentialStatusSnapshot.fromJson(Map<String, Object?> json) {
    return CredentialStatusSnapshot(
      credentialStatusSnapshotId: json['credentialStatusSnapshotId'] as String,
      schoolAccountId: (json['schoolAccountId'] ?? json['tenantId']) as String,
      studentProfileId: json['studentProfileId'] as String,
      identityCredentialId: json['identityCredentialId'] as String,
      credentialType: json['credentialType'] as String,
      credentialStatus: json['credentialStatus'] as String,
      validFrom: DateTime.parse(json['validFrom'] as String).toUtc(),
      validUntil: json['validUntil'] == null ? null : DateTime.parse(json['validUntil'] as String).toUtc(),
      revokedAt: json['revokedAt'] == null ? null : DateTime.parse(json['revokedAt'] as String).toUtc(),
      snapshotGeneratedAt: DateTime.parse(json['snapshotGeneratedAt'] as String).toUtc(),
      snapshotExpiresAt: DateTime.parse(json['snapshotExpiresAt'] as String).toUtc(),
    );
  }

  Map<String, Object?> toJson() {
    return {
      'credentialStatusSnapshotId': credentialStatusSnapshotId,
      'schoolAccountId': schoolAccountId,
      'studentProfileId': studentProfileId,
      'identityCredentialId': identityCredentialId,
      'credentialType': credentialType,
      'credentialStatus': credentialStatus,
      'validFrom': validFrom.toUtc().toIso8601String(),
      'validUntil': validUntil?.toUtc().toIso8601String(),
      'revokedAt': revokedAt?.toUtc().toIso8601String(),
      'snapshotGeneratedAt': snapshotGeneratedAt.toUtc().toIso8601String(),
      'snapshotExpiresAt': snapshotExpiresAt.toUtc().toIso8601String(),
    };
  }
}
