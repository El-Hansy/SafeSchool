import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/documents/documents.dart';

void main() {
  test('document storage lists guardian-visible documents', () async {
    final repo = DocumentsRepository();
    final documents = await repo.documents();

    expect(documents.single.reference, 'DOC-1');
    expect(documents.single.title, 'Consent form');
  });
}
