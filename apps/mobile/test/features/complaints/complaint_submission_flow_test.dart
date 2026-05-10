import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/complaints/complaints.dart';
void main() { test('complaint_submission_flow_test', () async { final repo = ComplaintRepository(); final ref = await repo.submit(const ComplaintDraft(studentProfileId: 'student-1', categoryCode: 'safety', description: 'Concern', clientRequestId: '1')); expect(ref, startsWith('CMP-')); }); }
