import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/documents/documents.dart';

void main() {
  test('document fixture carries reference and title', () {
    const document = DocumentSummary('DOC-1', 'Consent form');

    expect(document.reference, 'DOC-1');
    expect(document.title, 'Consent form');
  });
}
