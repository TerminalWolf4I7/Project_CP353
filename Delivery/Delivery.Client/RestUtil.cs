// Utility กลางสำหรับเรียก REST API ของระบบ
//
// Architecture:
// WinForms UI
// ↓
// RestUtil (HTTP Client Layer)
// ↓
// ASP.NET Web API (localhost:5000/api/)
//
// หน้าที่หลัก:
// - รวม logic การเรียก API ไว้จุดเดียว
// - ลด code ซ้ำของ HttpClient
// - จัดการ serialize/deserialize JSON อัตโนมัติ
// - ใช้ร่วมกันทุก Form ในระบบ

using System.Net.Http.Json;

namespace Delivery.Client
{
    public static class RestUtil
    {
        // HttpClient กลางของทั้ง application
        //
        // ใช้ static/shared instance เพื่อ:
        // - reuse TCP connection
        // - ลด socket exhaustion
        // - ลด overhead ของการสร้าง HttpClient ใหม่บ่อย ๆ
        //
        // BaseAddress:
        // ทุก request จะถูก prepend ด้วย:
        // http://localhost:5000/api/
        //
        // ตัวอย่าง:
        // endpoint = "orders"
        // → final URL = http://localhost:5000/api/orders
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };

        // =====================================================
        // GET JSON → DESERIALIZE
        // =====================================================

        // GET request แล้ว deserialize JSON response เป็น object type T
        //
        // Workflow:
        // 1. ส่ง GET request ไป endpoint
        // 2. อ่าน response body
        // 3. deserialize JSON เป็น object type T
        //
        // Input:
        // - endpoint: path ของ API เช่น "orders/1"
        //
        // Output:
        // - object type T หรือ null
        //
        // หมายเหตุ:
        // GetFromJsonAsync จะ throw exception อัตโนมัติ
        // ถ้า request fail หรือ deserialize ไม่สำเร็จ
        //
        // เหมาะสำหรับ:
        // - request ที่คาดหวัง success เป็นหลัก
        // - ไม่ต้องจัดการ status code เอง
        public static async Task<T?> GetAsync<T>(string endpoint)
        {
            return await _client.GetFromJsonAsync<T>(endpoint);
        }

        // =====================================================
        // GET RAW RESPONSE
        // =====================================================

        // GET request แล้วคืน HttpResponseMessage ดิบ
        //
        // ใช้เมื่อ caller ต้องการ:
        // - เช็ค status code เอง
        // - handle 404/409/401 เอง
        // - อ่าน response body เองภายหลัง
        //
        // Output:
        // - HttpResponseMessage แบบ raw
        //
        // หมายเหตุ:
        // method นี้จะไม่ throw อัตโนมัติถ้า HTTP status เป็น error
        public static async Task<HttpResponseMessage> GetResponseAsync(string endpoint)
        {
            return await _client.GetAsync(endpoint);
        }

        // =====================================================
        // POST JSON + AUTO DESERIALIZE
        // =====================================================

        // POST request พร้อม JSON body
        // แล้ว deserialize response body กลับเป็น TResponse
        //
        // Workflow:
        // 1. serialize body → JSON
        // 2. POST ไปยัง API
        // 3. เช็ค status code
        // 4. deserialize response body
        //
        // Input:
        // - endpoint: path ของ API
        // - body: object ที่ต้องการส่ง
        //
        // Output:
        // - object type TResponse หรือ null
        //
        // Side effects:
        // - throw exception ถ้า status code ไม่สำเร็จ
        //
        // เหมาะสำหรับ:
        // - API ที่ return object กลับมา
        // - login/create resource
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest body)
        {
            // ส่ง JSON body ไป API
            var response = await _client.PostAsJsonAsync(endpoint, body);

            // ถ้า status code ไม่ใช่ success จะ throw exception
            response.EnsureSuccessStatusCode();

            // deserialize response body เป็น object
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        // =====================================================
        // POST JSON → RAW RESPONSE
        // =====================================================

        // POST request พร้อม JSON body
        // แต่คืน HttpResponseMessage ดิบกลับมา
        //
        // ใช้เมื่อ:
        // - caller ต้องการ handle status code เอง
        // - response body ไม่แน่นอน
        // - อยากอ่าน headers/status เอง
        //
        // หมายเหตุ:
        // method นี้จะไม่ throw อัตโนมัติ
        public static async Task<HttpResponseMessage> PostResponseAsync<TRequest>(
            string endpoint,
            TRequest body)
        {
            return await _client.PostAsJsonAsync(endpoint, body);
        }

        // =====================================================
        // POST WITHOUT BODY
        // =====================================================

        // POST request แบบไม่มี body
        //
        // ใช้กับ endpoint ที่ trigger action เฉย ๆ
        // เช่น:
        // - approve
        // - refresh
        // - trigger process
        //
        // Output:
        // - raw HttpResponseMessage
        public static async Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            return await _client.PostAsync(endpoint, null);
        }

        // =====================================================
        // PUT JSON
        // =====================================================

        // PUT request พร้อม JSON body
        //
        // ใช้สำหรับ:
        // - update resource ทั้งก้อน
        // - replace data
        //
        // ตัวอย่าง:
        // PUT /restaurants/1
        //
        // Output:
        // - raw HttpResponseMessage
        public static async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint,
            T body)
        {
            return await _client.PutAsJsonAsync(endpoint, body);
        }

        // =====================================================
        // PATCH JSON
        // =====================================================

        // PATCH request พร้อม JSON body
        //
        // ใช้สำหรับ:
        // - update บาง field
        // - partial update
        //
        // ตัวอย่าง:
        // PATCH /orders/1/status
        //
        // Output:
        // - raw HttpResponseMessage
        public static async Task<HttpResponseMessage> PatchAsync<T>(
            string endpoint,
            T body)
        {
            return await _client.PatchAsJsonAsync(endpoint, body);
        }

        // =====================================================
        // PATCH WITHOUT BODY
        // =====================================================

        // PATCH request แบบไม่มี body
        //
        // ใช้กับ endpoint ที่:
        // - เปลี่ยน state
        // - trigger action
        // โดยไม่ต้องส่ง payload เพิ่ม
        public static async Task<HttpResponseMessage> PatchAsync(string endpoint)
        {
            return await _client.PatchAsync(endpoint, null);
        }

        // =====================================================
        // DELETE RESOURCE
        // =====================================================

        // DELETE request สำหรับลบ resource
        //
        // ตัวอย่าง:
        // DELETE /orders/1
        //
        // Output:
        // - raw HttpResponseMessage
        //
        // หมายเหตุ:
        // caller ต้องเช็ค success เองผ่าน:
        // response.EnsureSuccessStatusCode()
        public static async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _client.DeleteAsync(endpoint);
        }

        // =====================================================
        // READ JSON FROM EXISTING RESPONSE
        // =====================================================

        // deserialize JSON จาก HttpResponseMessage ที่ caller มีอยู่แล้ว
        //
        // ใช้เมื่อ:
        // - caller handle status code เองก่อน
        // - แล้วค่อยอ่าน body ภายหลัง
        //
        // Workflow:
        // 1. caller ส่ง request เอง
        // 2. caller เช็ค response.StatusCode เอง
        // 3. เรียก ReadAsAsync<T>() เพื่ออ่าน body
        //
        // ตัวอย่าง:
        // var response = await GetResponseAsync(...);
        // if (response.IsSuccessStatusCode)
        // {
        //     var data = await ReadAsAsync<MyDto>(response);
        // }
        //
        // Output:
        // - object type T หรือ null
        public static async Task<T?> ReadAsAsync<T>(HttpResponseMessage response)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}