using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace ChatGPTWinForms
{
    public enum TipoLog
    {
        DEBUG,
        INFO,
        WARN,
        ERROR
    }
    public partial class Form1 : Form
    {
        List<Mensaje> ListaMensajes = new List<Mensaje>();
        const int MAX_PROMPT_LENGTH = 100;
        const int MAX_MSJ = 5;
        int msj_lista_tamaño = 0;
        public Form1()
        {
            InitializeComponent();
        }

        //**************************** BOTONES DEL FORMS *****************************************
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (txtPromptEstaVacio()) { SetMensajeEstadoError("El prompt está vacío"); return; }
            if (getPrompt() == getUltimoMsj()) { SetMensajeEstadoError("Mensaje duplicado, envié otro"); return; }
            if (msj_lista_tamaño >= MAX_MSJ) ListaMensajes.RemoveAt(0);
            addMsjChat(getPrompt());
            EnviarPrompt(getPrompt());

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            ListaMensajes.Clear();
            txtChat.Clear();
            txtPrompt.Clear();
            SetMensajeEstado("Limpiado.");
        }

        //************************** SECCIÓN DEL ENVIADO DE PROMPTS ******************************
        async private Task EnviarPrompt(string msj)
        {
            try
            {
                SetMensajeEstado("Enviando...");
                cambiarEstadoBtn(false);
                string? msj_output = await OpenAiAPI.apiAi(msj, ListaMensajes);

                if (msj_output.StartsWith("4")
                    || msj_output.StartsWith("5")
                    || msj_output.StartsWith("HTTP") ) { SetMensajeEstadoError(msj_output); cambiarEstadoBtn(true); return; }

                addMsjAsistente(msj_output);
                SetMensajeEstado("Listo.");
            }
            catch (TaskCanceledException) { Log.AddLinea("Error de conexión intentalo de nuevo", TipoLog.ERROR); SetMensajeEstadoError("Error de conexión intentalo de nuevo"); }
            catch (HttpRequestException) { Log.AddLinea("No se pudo conectar. Intenta de nuevo", TipoLog.ERROR); SetMensajeEstadoError("No se pudo conectar.Intenta de nuevo"); }
            catch (TimeoutException) { Log.AddLinea("Límite de espera alcanzado para la solicitud", TipoLog.ERROR); SetMensajeEstadoError("Límite de espera alcanzado para la solicitud"); }
            catch (Exception ex) { Log.AddLinea("Se ha producido un error al conectar con el API" + " Error: " + ex.StackTrace + " MSJ:" + ex.Message, TipoLog.ERROR); SetMensajeEstadoError(ex.Message); }
            
            cambiarEstadoBtn(true);
        }

        private void addMsjChat(string msj)
        {
            ListaMensajes.Add(new Mensaje
            {
                Role = "user",
                Content = msj
            });

            txtChat.AppendText("Tú: " + msj + "\r\n");
            msj_lista_tamaño++;
        }

        private void addMsjAsistente(string msj)
        {
            ListaMensajes.Add(new Mensaje
            {
                Role = "system",
                Content = msj
            });

            txtChat.AppendText("Asistente: " + msj + "\r\n");
            msj_lista_tamaño++;
        }


        //************************** METÓDOS DE AYUDA DEL FORMS **********************************
        private void SetMensajeEstado(string msj)
        {
            lblEstado.ForeColor = Color.White;
            lblEstado.Text = msj;
        }

        private void SetMensajeEstadoError(string msj)
        {
            lblEstado.ForeColor = Color.Red;
            lblEstado.Text = msj;
        }
        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {

        }

        private void cambiarEstadoBtn(bool estado)
        {
            btnEnviar.Enabled = estado;
            btnLimpiar.Enabled = estado;
        }

        private bool txtPromptEstaVacio() => string.IsNullOrWhiteSpace(getPrompt());

        private string getPrompt() => txtPrompt.Text;

        private string getUltimoMsj() { if (ListaMensajes.Count > 0) return ListaMensajes[msj_lista_tamaño - 1].Content; else return ""; }
    }

    // La clase mensaje sirve para guardar la información de los mensajes
    public class Mensaje
    {
        [JsonPropertyName("role")]
        public required string Role { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }
    }

    // Creación de la clase Log
    public class Log
    {

        // Añade una línea al log, junto con el tipo
        public static void AddLinea(string linea, TipoLog tipo)
        {
            System.IO.File.AppendAllText("Log.txt", $"{DateTime.Now.ToString()}" + $" -{tipo}-".PadRight(8) + $" {linea}\n");
        }

        // Añade una línea al log
        public static void AddLinea(string linea)
        {

            System.IO.File.AppendAllText("Log.txt", $"{DateTime.Now.ToString()} -INFO-  {linea}\n");
        }

        // Se crea el archivo log
        public static void IniciarLog()
        {
            System.IO.File.WriteAllText("Log.txt", ""); // Crea el archivo log, en caso de existir, entonces borra su contenido
            Log.AddLinea("Programa iniciado");
        }

    }

    // La clase OpenAiAPI tiene el método estático para conectarse a la API
    public class OpenAiAPI
    {

        public static async Task<string?> apiAi(string msj, List<Mensaje> ListaMensajes)
        {
            try
            {
                using var client = new HttpClient();
                var apiString = System.IO.File.ReadAllText(@"C:\apikey.txt");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiString}");
                var body = new
                {
                    model = "gpt-4o",
                    messages = ListaMensajes.ToArray()
                };
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Log.AddLinea("Iniciado conexión con el api de OpenAI...");
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                
                // En caso de que sea  exitosa la respuesta entonces lee el contenido
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Log.AddLinea("Conexión con el API de OpenAi establecida");
                    using var doc = JsonDocument.Parse(result);
                    var message = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();
                    return message;
                }
                // En caso de no ser exitosa la respesta entonces retorna el código de error
                else if (response.StatusCode == HttpStatusCode.BadRequest)      // 400 Bad Request
                {
                    return "400 Bad Request: Error, no se pudo procesar la solicitud";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)    // 401 Unauthorized
                {
                    return "401 Unauthorized: Error, API key inválida o ausente";
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429 TooManyRequests
                {
                    return "429 TooManyRequests: Error, límite alcanzado, espera unos segundos ";
                }
                else if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600) // 5xx Internal Server Error
                {
                    return "5xx Internal Server Error: Servicio ocupado. Reintentar ";
                }
                else
                {
                    return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
                }

            }
            // El try catch atrapa todas las excepciones y entonces tira de nuevo la excepción para que la función principal la capte e imprima el mensaje.
            catch (Exception){ throw; } 
        }

    }


}
