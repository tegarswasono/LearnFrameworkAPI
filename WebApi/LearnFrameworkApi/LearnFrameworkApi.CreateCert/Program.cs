// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

var savePath = Path.Combine($"C:\\", "certs", $"{DateTime.Now:yyyyMMddHHmmss}.pfx");

GenerateSelfSignedEncryptionCertificate("CN=Learn Framework API Project", savePath);

static void GenerateSelfSignedEncryptionCertificate(string subjectName, string fileName, int keySizeInBits = 2048, int validityYears = 5)
{
    using (var algorithm = RSA.Create(keySizeInBits))
    {
        var subject = new X500DistinguishedName(subjectName);
        var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

        var notBefore = DateTimeOffset.UtcNow;
        var notAfter = notBefore.AddYears(validityYears);

        var certificate = request.CreateSelfSigned(notBefore, notAfter);

        // Save the certificate to a file
        File.WriteAllBytes(fileName, certificate.Export(X509ContentType.Pfx, "WFR@indonesia123"));
    }
}