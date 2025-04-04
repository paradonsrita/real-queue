﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using QMS.Models;

namespace QMS.Services
{
    public class QueueService
    {
        private readonly HttpClient _httpClient;

        public QueueService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Queue");
        }

        // ฟังก์ชันเพื่อเพิ่มคิวใหม่
        public async Task<QueueModel> AddQueueAsync(string queueType)
        {

            var response = await _httpClient.PostAsync($"/api/Queue?type={queueType}", null);

            if (response.IsSuccessStatusCode)
            {
                var newQueue = await response.Content.ReadFromJsonAsync<QueueModel>();
                
                    return newQueue; // คืนค่าข้อมูล QueueResponse
                
                
            }

            else
            {
                Console.WriteLine($"Error adding queue: {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }

        public async Task<QueueModel> GetQueueById(int queueId)
        {

            try
            {
                var response = await _httpClient.GetAsync($"/api/Queue/{queueId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var newQueue = await response.Content.ReadFromJsonAsync<QueueModel>();
                    return newQueue; // คืนค่าข้อมูล QueueResponse
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API returned error: {response.StatusCode} - {errorMessage}");
                return null;
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                return null;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error fetching queue details: {ex.Message}");
                return null;
            }
        }

    }
}

