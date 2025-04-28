# TOTP Demo

## Overview

This project is a Time-based One-Time Password (TOTP) implementation in C#. TOTP is a widely used algorithm for generating temporary authentication codes that are valid for a short period of time, commonly used in two-factor authentication (2FA) systems.

The solution consists of three projects:

- **TotpShared**: Contains the core TOTP implementation
- **TotpGenerator**: A console application that generates TOTP codes
- **TotpClient**: A console application that validates TOTP codes

It's a companion to [my post](https://ronnydelgado.com/my-blog/time-based-one-time-passwords-a-hands-on-guide), where I dive deeper into the why and how.

## Features

- TOTP code generation with Base32 secret key
- Real-time code validity countdown
- Code validation with time drift consideration (±1 time step)
- Default 30-second time step interval
- 6-digit TOTP codes

## Project Structure

### TotpShared

The core library containing the TOTP implementation:

- **TotpService.cs**: Main service implementing TOTP generation and validation
- **SecretKey.cs**: Contains a shared demo secret key

### TotpGenerator

A console application that continuously displays TOTP codes:

- Generates and displays current TOTP code
- Shows remaining validity time in seconds
- Updates in real-time

### TotpClient

A console application for validating TOTP codes:

- Accepts user input of 6-digit codes
- Validates codes against the same shared secret
- Provides immediate validation feedback

## How to Use

### Running the Generator

1. Execute the TotpGenerator application
2. The application will display the current TOTP code with its remaining validity time
3. The code automatically updates when the time step expires
4. Press Ctrl+C to exit

### Running the Client

1. Execute the TotpClient application
2. Enter a 6-digit TOTP code when prompted
3. The application will validate the code and display whether it's valid
4. Type 'exit' to quit the application

### Running both at the same time

1. Open two terminal windows in the **`TotpDemo`** directory.
2. In the first terminal, run the generator:
   ```bash
   cd TotpGenerator/
   dotnet run
   ```
3. In the second terminal, run the client:
   ```bash
   cd TotpClient/
   dotnet run
   ```

## Implementation Notes

- Uses HMAC-SHA1 for secure hash generation
- Includes Base32 decoding for secret keys
- Supports configurable time step duration (default: 30 seconds)
- Supports configurable code length (default: 6 digits)
- Implements validation with time drift allowance of ±1 time step

## Security Considerations

This is a demonstration project only. In a production environment:

- Secret keys should not be hardcoded but securely stored and managed
- Consider implementing additional security features such as rate limiting
- For high-security applications, consider additional factors beyond TOTP

## Requirements

- .NET 9.0
- C# 13.0

