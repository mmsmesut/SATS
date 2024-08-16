using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SATS.Core
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userId, string userEmail)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

            //JWT (JSON Web Token) içinde Claim : tokenin taşıdığı bilgileri ifade eder, Bu Claims'ler, kullanıcı hakkında belirli bilgileri içerir
            //ve token'ı alan taraflar bu bilgileri doğrulamak veya işlem yapmak için kullanabilir.
            //2-PAYLOAD
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),//Sub -> token'ın hangi kullanıcıya ait olduğunu belirtmek için kullanılır. ,userId gibi benzersiz bir kullanıcı kimliği olabilir.
               //Zorunlu Değil: Ancak, genellikle token'ın kim için oluşturulduğunu belirtmek için kullanılır. Eğer bu tür bir kimlik bilgisine ihtiyacınız yoksa, eklemeyebilirsiniz.
                new Claim(JwtRegisteredClaimNames.Email,userEmail),// kullanıcının e-posta adresini taşır. Kullanıcının kimliğini doğrulamak veya e-posta tabanlı işlemler için kullanılabilir.
                //Zorunlu değil
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                // Token'ın benzersiz bir kimliğini temsil eder. Her token için farklı bir JTI değeri oluşturulur. Bu, token'ın tekrarlanmasını veya tekrar kullanılmasını önlemek için faydalıdır.
                //Zorunlu Değil
            };

            //imza için kullanılır 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            //1. SymmetricSecurityKey
            //SymmetricSecurityKey : bir simetrik anahtarla çalışan güvenlik algoritmaları için kullanılan anahtarı temsil eder.
            //secretKey, sizin belirlediğiniz bir gizli anahtardır ve token'ın güvenliğini sağlar.
            //secretKey'i UTF-8 formatında byte dizisine dönüştürür. Çünkü kriptografik işlemler genellikle byte düzeyinde yapılır.

            //JWT'yi imzalamak için bir güvenlik anahtarına ihtiyaç duyarsınız. Bu anahtar, token'ı imzalama ve daha sonra doğrulama sürecinde kullanılır.
            //Eğer biri JWT token'ını değiştirirse, imza doğrulama başarısız olur, bu da token'ın güvenilmez olduğunu gösterir.
            //Simetrik anahtar, hem token'ı imzalamak hem de doğrulamak için kullanılır. Bu nedenle, aynı anahtar hem sunucuda
            //hem de doğrulama yapacak diğer sistemlerde saklanmalıdır.

            //1-HEADER: Algoritma ve tip bilgisi içerir.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //3. SIGNATURE (İmza)
            var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryMinutes),
            signingCredentials: creds);

            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VySWQiLCJlbWFpbCI6InVzZXJAZW1haWwuY29tIiwianRpIjoiYXVuaXF1ZS1pZCJ9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
            /*
                eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9: Header'ın base64 kodlu hali.
                eyJzdWIiOiJ1c2VySWQiLCJlbWFpbCI6InVzZXJAZW1haWwuY29tIiwianRpIjoiYXVuaXF1ZS1pZCJ9: Payload'un base64 kodlu hali.
                SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c: Signature'ın base64 kodlu hali.
             */

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}

/*
 Claims'lerin Kullanımı ve Zorunluluğu
JWT içinde Claims'ler zorunlu değildir;
ancak token'ın içeriği ve amacı doğrultusunda belirli claim'ler eklenebilir. 
Kullanıcı kimliği, 
rol bilgisi,
e-posta gibi bilgileri taşımak isterseniz, bu claim'leri ekleyebilirsiniz. 
Bununla birlikte, token'ı oluştururken ve doğrularken hangi bilgilerin gerekli olduğuna karar vermelisiniz.

Özetle, bu claim'ler zorunlu değildir, 
ancak uygulamanızın ihtiyaçlarına göre belirli bilgiler taşımak ve güvenlik önlemlerini artırmak için kullanılması tavsiye edilir. 
Eğer uygulamanızda ihtiyaç yoksa bu claim'leri eklemeyebilirsiniz veya token'ınıza eklemek için kendi özel claim'lerinizi tanımlayabilirsiniz.
 */

/*
 
JWT İmzalama Nedir?
JWT, üç ana bileşenden oluşur:

Header (Başlık): JWT'nin türü ve kullanılan imzalama algoritması hakkında bilgi içerir.

Payload (Yük): Token içinde taşınan veriler (örneğin kullanıcı bilgileri, roller vb.) yer alır.

Signature (İmza): Token'ın geçerliliğini ve bütünlüğünü doğrulamak için kullanılır.

JWT'yi imzalamak, Header ve Payload bileşenlerinin birleştirilip belirli bir algoritma ile Signature bölümünün oluşturulması işlemidir.

JWT İmzalama Süreci
Header ve Payload'ın Birleştirilmesi:

Header ve Payload base64 olarak kodlanır ve birleştirilir.
plaintext
Kodu kopyala
Base64UrlEncode(header) + "." + Base64UrlEncode(payload)
İmzanın Oluşturulması:

Bu birleşim, belirli bir algoritma ve bir gizli anahtar (secret key) kullanılarak imzalanır.
plaintext
Kodu kopyala
HMACSHA256(
  base64UrlEncode(header) + "." + base64UrlEncode(payload),
  secret
)
JWT'nin Tamamlanması:

İmza da base64 olarak kodlanır ve Header ve Payload ile birleştirilir. Sonuç olarak, JWT şu şekilde olur:
plaintext
Kodu kopyala
{header}.{payload}.{signature}
İmzalamanın Amacı ve Önemi
Güvenlik: JWT imzalandığında, token'ın içeriği (özellikle Payload kısmı) sonradan değiştirilirse, imza geçersiz hale gelir. Bu sayede token'ın yetkisiz kişiler tarafından değiştirilmesi önlenir.

Doğrulama: Token'ı alan her taraf, token'ın imzasını kontrol ederek bu token'ın gerçekten belirlenen kaynak (örneğin bir API sunucusu) tarafından oluşturulup oluşturulmadığını ve içeriğin değiştirilip değiştirilmediğini doğrulayabilir.

Bütünlük: İmza, token'ın bütünlüğünü sağlar. Eğer JWT'nin içeriği (örneğin Payload) değiştirilirse, imza doğrulaması başarısız olur ve bu token geçersiz kabul edilir.

İmza Algoritmaları
JWT'yi imzalamak için farklı algoritmalar kullanılabilir. En yaygın olanlar:

HMAC + SHA256 (HS256): Simetrik bir algoritmadır. Aynı gizli anahtar, hem imzalama hem de doğrulama için kullanılır.
RS256: Asimetrik bir algoritmadır. Özel bir anahtar imzalamak için, açık bir anahtar ise doğrulama için kullanılır.
Özet
JWT'yi imzalamak, token'ın kimliğini doğrulamak ve içeriğinin bütünlüğünü sağlamak için yapılan kriptografik bir işlemdir. Bu sayede, token'ların güvenilir ve müdahale edilmemiş olduğunu garanti edersiniz. İmzalanmamış bir JWT, güvenli kabul edilmez çünkü herkes tarafından değiştirilebilir ve geçersiz kılınamaz.


Payload JSON Formatında:

{
  "sub": "userId",       // Kullanıcı kimliği
  "email": "userEmail",  // Kullanıcı e-posta adresi
  "jti": "unique-id",    // Benzersiz kimlik
  // Diğer Claims (Varsa)
}
 
 */