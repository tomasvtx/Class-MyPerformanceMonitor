# Class-MyPerformanceMonitor
Třída pro zlištění stavu CPU, RAM, disku, LAN, ...

.NET Framework 2.0

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Používání knihovny PapouchSensorTM</title>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            background-color: #f4f4f4;
            padding: 20px;
            text-align: justify;
        }
        pre {
            max-width: 800px;
            margin: 0 auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            white-space: pre-wrap;
            word-wrap: break-word;
        }
    </style>
</head>
<body>
    <h1>Používání knihovny MyPerformanceMonitor
</h1>

    <p>Vypadá to, že máte kód pro monitorování systémových údajů v jazyce C#. Následující jsou některé základní pokyny pro používání této knihovny:</p>

    <h2>1. Inicalizace knihovny:</h2>
    <pre>
        <code>
SystemData systemData = new SystemData();</code>
    </pre>

    <h2>2. Získání informací o procesoru:</h2>
    <pre>
        <code>
double cpuUsage = systemData.GetProcessorData();
Console.WriteLine($"Procesor: {cpuUsage}%");</code>
    </pre>

 <h2>3. Získání informací o paměti:</h2>
    <pre>
        <code>
string memoryUsage = systemData.GetMemoryVData();
Console.WriteLine($"Využití paměti: {memoryUsage}");
</code>
    </pre>
    
    <h2>4. Získání informací o fyzické paměti:</h2>
    <pre>
        <code>
string physicalMemoryUsage = systemData.GetMemoryPData();
Console.WriteLine($"Využití fyzické paměti: {physicalMemoryUsage}");</code>
    </pre>

<h2>5. Získání informací o síti</h2>
    <pre>
        <code>
double receivedData = systemData.GetNetData(SystemData.NetData.Received);
double sentData = systemData.GetNetData(SystemData.NetData.Sent);

Console.WriteLine($"Přijato: {receivedData} bytes");
Console.WriteLine($"Odesláno: {sentData} bytes");
</code>
    </pre>

<h2>6. Získání informací o disku:</h2>
    <pre>
        <code>
double diskRead = systemData.GetDiskData(SystemData.DiskData.Read);
double diskWrite = systemData.GetDiskData(SystemData.DiskData.Write);

Console.WriteLine($"Disk - Čtení: {diskRead} bytes");
Console.WriteLine($"Disk - Zápis: {diskWrite} bytes");
</code>
    </pre>
    
    <h2>7. Získání informací o logických discích:</h2>
    <pre>
        <code>
string logicalDiskInfo = systemData.LogicalDisk();
Console.WriteLine($"Logické disky: {logicalDiskInfo}");
</code>
    </pre>
    
    
    <h2>8. Získání systémového reportu:</h2>
    <pre>
        <code>
var systemReport = Program.GetSystemReport();

Console.WriteLine($"Uživatel: {systemReport.Uzivatel}");
Console.WriteLine($"CPU: {systemReport.CPU}");
Console.WriteLine($"Paměť a swap soubory: {systemReport.MemorySwapFileString}");
Console.WriteLine($"Model PC: {systemReport.ModelPC}");
Console.WriteLine($"Čtení a zápis z disku: {systemReport.DiskWriteRead}");
Console.WriteLine($"Odeslaná a přijatá data sítě: {systemReport.NetSentAndReceived}");
</code>
    </pre>

    <p>Toto jsou pouze základní příklady a můžete je dále upravovat nebo rozšiřovat podle potřeby. Mějte na paměti, že tyto informace mohou být citlivé a není vhodné je sdílet bezpečným nebo nesprávným způsobem.</p>

</body>
</html>
