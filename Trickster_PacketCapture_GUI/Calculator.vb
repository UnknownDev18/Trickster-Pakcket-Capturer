Imports System.Convert
Module Calculator
    Public PacketDic() As Byte = IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory & "\dictionary.key")
    Public Function ByteToHex(ByRef Origin As Byte) As String
        Dim ns As String = Origin.ToString("X")
        If ns.Length = 1 Then
            ns = "0" & ns
        End If
        Return ns
    End Function
    Public Function BytesToHex(ByRef Bytes() As Byte) As String
        Dim result As String = "", ns As String
        For i As Integer = 0 To UBound(Bytes)

            If result <> "" Then
                result += " "
            End If
            ns = Bytes(i).ToString("X")
            If Len(ns) = 1 Then
                ns = "0" & ns
            End If
            result += ns

        Next

        Return result
    End Function
    Public Function HIBYTE(ByRef Num As Int32) As Byte
        Return ((ToInt32(&HFF00) And Num) >> 8)
    End Function
    Public Function LOBYTE(ByRef Num As Int32) As Byte
        Return GetByte(Num)
    End Function
    Public Function Make_BufferContent(ByRef buffer() As Byte) As Byte()

        Dim count As Integer = 0
        For i As Integer = 0 To UBound(buffer)
            count = i
            If i >= 2 Then
                If buffer(i - 2) = 4 AndAlso buffer(i - 1) = 0 AndAlso buffer(i) = 0 Then
                    count -= 3
                    Exit For
                ElseIf buffer(i - 2) = 0 AndAlso buffer(i - 1) = 0 AndAlso buffer(i) = 0 Then
                    count -= 3
                    Exit For
                End If
            End If
        Next

        Dim nf() As Byte
        ReDim nf(count)
        For i As Integer = 0 To count
            nf(i) = buffer(i)
        Next
        Return nf
    End Function
    Public Function StrToBytes(Origin_Str As String) As Byte()
        Dim Origin() As String = Split(Origin_Str, " ")
        Dim T() As Byte
        ReDim T(UBound(Origin))
        For i As Integer = 0 To UBound(T)
            T(i) = Val(HexToByte(Origin(i)))
        Next
        Return T
    End Function
    Private Function HexToByte(Origin As String) As Byte
        Dim HexData As String = "0123456789ABCDEF"
        Dim BData As Byte
        Dim BData_1 As Byte
        For i As Integer = 0 To 15
            If Origin(0) = HexData(i) Then
                BData = i
            End If
            If Origin(1) = HexData(i) Then
                BData_1 = i
            End If
        Next

        Dim Result As Byte = (16 * BData) + BData_1
        Return Result

    End Function
    Public Function Combine(ByVal a As Byte(), ByVal b As Byte()) As Byte()
        Dim c As Byte() = New Byte(a.Length + b.Length - 1) {}
        System.Buffer.BlockCopy(a, 0, c, 0, a.Length)
        System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length)
        Return c
    End Function
    Public Function Combine(ByVal a As Byte(), ByVal b As Byte(), ByVal c As Byte()) As Byte()
        Return Combine(Combine(a, b), c)
    End Function

    Public Function GetByte(Origin As Int32) As Byte
        Return (ToInt32(Origin >> 8) << 8) Xor Origin
    End Function
    Public Function GetInt16(Origin As Int64) As Int16
        Return (ToInt64(&HFFFF) And Origin)
    End Function
    Public Function Packet_GetLength(ByRef Packet() As Byte, ByRef Key1 As Byte) As UInt16

        Dim OpBits(8) As Byte, PacketLength As UInt16
        OpBits(0) = PacketDic((ToInt32(Key1 Xor Packet(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(Packet(7) Xor Packet(6)) << 8) + Packet(1))

        PacketLength = ToUInt16((ToUInt16(OpBits(1)) << 8) + OpBits(0))
        Return PacketLength

    End Function
    Public Function Packet_CheckHeader(ByRef Packet() As Byte, ByRef Key1 As Byte) As Boolean

        Dim OpBits(8) As Byte, Key2 As Int32
        OpBits(4) = PacketDic((ToInt32(Key1 Xor Packet(2)) << 8) + Packet(4))
        OpBits(5) = PacketDic((ToInt32(Packet(7) Xor Packet(3)) << 8) + Packet(5))
        OpBits(2) = PacketDic((ToInt32(Packet(0) Xor Packet(6)) << 8) + Packet(2))
        OpBits(3) = PacketDic((ToInt32(Key1 Xor Packet(1)) << 8) + Packet(3))
        OpBits(0) = PacketDic((ToInt32(Key1 Xor Packet(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(Packet(7) Xor Packet(6)) << 8) + Packet(1))
        OpBits(7) = PacketDic((ToInt32(Packet(6)) << 8) + Packet(7)) Xor 2
        OpBits(6) = Packet(6)

        If OpBits(4) <> 0 Or OpBits(5) <> 0 Then
            Return False
        End If

        Dim Ischeck_Header As Boolean = False
        Dim v3 As Byte = PacketDic(PacketDic((ToInt32(OpBits(0)) << 8) + OpBits(2)) + (ToInt32(Key1) << 8))
        Dim v6 As Byte = PacketDic(PacketDic((ToInt32(OpBits(3)) << 8) + OpBits(1)) + (ToInt32(OpBits(6)) << 8))
        OpBits(8) = PacketDic((ToInt32(v3) << 8) + v6)
        Key2 = (ToInt32(OpBits(3)) << 8) + OpBits(2)

        If OpBits(8) = Packet(8) Then
            Ischeck_Header = True
        End If

        Return Ischeck_Header

    End Function
    Public Function Packet_Encrypt_ToServer(ByRef Packet() As Byte) As Byte()

        Dim OpBits(8) As Byte, Key1 As Byte = 1, Key2 As Int32


        Dim v3 As Byte = PacketDic(PacketDic((ToInt32(Packet(0)) << 8) + Packet(2)) + (ToInt32(Key1) << 8))
        Dim v6 As Byte = PacketDic(PacketDic((ToInt32(Packet(3)) << 8) + Packet(1)) + (ToInt32(Packet(6)) << 8))
        OpBits(8) = PacketDic((ToInt32(v3) << 8) + v6)

        OpBits(7) = PacketDic((ToInt32(Packet(6)) << 8) + Packet(7))
        OpBits(6) = Packet(6)
        OpBits(0) = PacketDic((ToInt32(Key1 Xor OpBits(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(OpBits(7) Xor OpBits(6)) << 8) + Packet(1))

        OpBits(2) = PacketDic((ToInt32(OpBits(0) Xor OpBits(6)) << 8) + Packet(2))
        OpBits(3) = PacketDic((ToInt32(Key1 Xor OpBits(1)) << 8) + Packet(3))

        OpBits(4) = PacketDic((ToInt32(Key1 Xor OpBits(2)) << 8) + Packet(4))
        OpBits(5) = PacketDic((ToInt32(OpBits(7) Xor OpBits(3)) << 8) + Packet(5))


        Key2 = (ToInt32(Packet(3)) << 8) + Packet(2)

        Dim CKey As Int32, ConBits() As Byte, cCount As Byte
        CKey = ToInt32(Packet(6)) * Key2
        cCount = Packet(6) Mod &HD
        ReDim ConBits(UBound(Packet) - 9)

        Dim a As Integer = 8, PKey As Int64
        For i As Integer = 0 To UBound(ConBits)
            a = 9 + i
            PKey += Packet(a)
            ConBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor i)) << 8) + Packet(a))
        Next

        Dim ExitBits() As Byte
        ReDim ExitBits(1 + cCount)
        If a = 8 Then
            ExitBits(0) = ToInt32(&HFF) And PKey
            ExitBits(1) = (ToInt32(&HFF00) And PKey) >> 8

            ExitBits(0) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor a - 8)) << 8) + ExitBits(0))
            ExitBits(1) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor a - 7)) << 8) + ExitBits(1))
        Else
            PKey = (ToUInt16(PacketDic((ToUInt16(LOBYTE(CKey)) << 8) + &H0)) << 8) + PacketDic(((ToUInt16(LOBYTE(CKey)) + &H1) << 8) + &H0)
            ExitBits(0) = HIBYTE(PKey)
            ExitBits(1) = LOBYTE(PKey)
        End If
        a += 2

        If cCount <> 0 Then
            For i As Integer = 0 To cCount - 1
                ExitBits(i + 2) = PacketDic((ToInt32(GetByte(CKey)) << 8) + (a + 1 + i))
            Next
        End If

        Return Combine(OpBits, ConBits, ExitBits)

    End Function
    Public Function Packet_Encrypt_ToClient(ByRef Packet() As Byte, ByRef Key1 As Byte) As Byte()

        Dim OpBits(8) As Byte, Key2 As Int32

        Dim v3 As Byte = PacketDic(PacketDic((ToInt32(Packet(0)) << 8) + Packet(2)) + (ToInt32(Key1) << 8))
        Dim v6 As Byte = PacketDic(PacketDic((ToInt32(Packet(3)) << 8) + Packet(1)) + (ToInt32(Packet(6)) << 8))
        OpBits(8) = PacketDic((ToInt32(v3) << 8) + v6)

        OpBits(7) = PacketDic((ToInt32(Packet(6)) << 8) + (Packet(7) Xor 2))
        OpBits(6) = Packet(6)
        OpBits(0) = PacketDic((ToInt32(Key1 Xor OpBits(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(OpBits(7) Xor OpBits(6)) << 8) + Packet(1))

        OpBits(2) = PacketDic((ToInt32(OpBits(0) Xor OpBits(6)) << 8) + Packet(2))
        OpBits(3) = PacketDic((ToInt32(Key1 Xor OpBits(1)) << 8) + Packet(3))

        OpBits(4) = PacketDic((ToInt32(Key1 Xor OpBits(2)) << 8) + Packet(4))
        OpBits(5) = PacketDic((ToInt32(OpBits(7) Xor OpBits(3)) << 8) + Packet(5))

        Key2 = (ToInt32(Packet(3)) << 8) + Packet(2)

        Dim CKey As Int32, ConBits() As Byte
        CKey = ToInt32(Packet(6)) * Key2
        ReDim ConBits(UBound(Packet) - 9)

        Dim a As Integer = 8, PKey As Int64
        For i As Integer = 0 To UBound(ConBits)
            a = 9 + i
            ConBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor i)) << 8) + Packet(a))
            PKey += Packet(a)
        Next

        Dim ExitBits() As Byte
        ReDim ExitBits(1)
        ExitBits(0) = ToInt32(&HFF) And PKey
        ExitBits(1) = (ToInt32(&HFF00) And PKey) >> 8

        ' byref
        Key1 = GetByte(ToInt32(PacketDic((ToInt32(ExitBits(0)) << 8) + Key1)) + Key1)

        ' Encrypt Exitbits
        ExitBits(0) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor a - 8)) << 8) + ExitBits(0))
        ExitBits(1) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor a - 7)) << 8) + ExitBits(1))
        a += 2

        Return Combine(OpBits, ConBits, ExitBits)

    End Function
    Public Function Packet_Decrypt_FromClient(ByRef Packet() As Byte, ByRef Key1 As Byte) As DecryptResult

        Dim OpBits(8) As Byte, Key2 As Int32
        OpBits(4) = PacketDic((ToInt32(Key1 Xor Packet(2)) << 8) + Packet(4))
        OpBits(5) = PacketDic((ToInt32(Packet(7) Xor Packet(3)) << 8) + Packet(5))
        OpBits(2) = PacketDic((ToInt32(Packet(0) Xor Packet(6)) << 8) + Packet(2))
        OpBits(3) = PacketDic((ToInt32(Key1 Xor Packet(1)) << 8) + Packet(3))
        OpBits(0) = PacketDic((ToInt32(Key1 Xor Packet(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(Packet(7) Xor Packet(6)) << 8) + Packet(1))
        OpBits(7) = PacketDic((ToInt32(Packet(6)) << 8) + Packet(7))
        OpBits(6) = Packet(6)

        Dim Ischeck_Header As Boolean = False
        Dim v3 As Byte = PacketDic(PacketDic((ToInt32(OpBits(0)) << 8) + OpBits(2)) + (ToInt32(Key1) << 8))
        Dim v6 As Byte = PacketDic(PacketDic((ToInt32(OpBits(3)) << 8) + OpBits(1)) + (ToInt32(OpBits(6)) << 8))
        OpBits(8) = PacketDic((ToInt32(v3) << 8) + v6)
        Key2 = (ToInt32(OpBits(3)) << 8) + OpBits(2)

        If OpBits(8) = Packet(8) Then
            Ischeck_Header = True
        End If

        Dim CKey As Int32, ConBits() As Byte, cCount As Byte
        CKey = ToInt32(OpBits(6)) * Key2
        cCount = OpBits(6) Mod &HD
        If (UBound(Packet) - cCount) = 9 Then
            cCount -= 1
        End If
        ReDim ConBits(UBound(Packet) - 9 - cCount - 2)

        Dim a As Integer = 8, PKey As Int64
        For i As Integer = 0 To UBound(ConBits)
            a = 9 + i
            ConBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor i)) << 8) + Packet(a))
            PKey += ConBits(i)
        Next

        Dim ExitBits() As Byte
        ReDim ExitBits(1 + cCount)
        For i As Integer = 0 To UBound(ExitBits)
            a += 1
            ExitBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor (a - 9))) << 8) + Packet(a))
        Next

        Dim pPair As Int32 = (ToInt32(ExitBits(1)) << 8) + ExitBits(0)
        Dim IsCheck1 As Boolean = False

        If PKey = pPair Then
            IsCheck1 = True
        End If

        Dim IsCheck2 As Boolean = True
        If cCount <> 0 Then
            For i As Integer = 0 To cCount - 1
                Dim ni As Byte = GetByte(a + 1 - cCount + i)
                Dim t1 As Byte = PacketDic((ToInt32(GetByte(CKey)) << 8) + ni)
                If Packet(ni) <> t1 Then
                    IsCheck2 = False
                End If
            Next
        End If

        Dim IsChecksum As Boolean = False
        If Ischeck_Header = True AndAlso IsCheck1 = True AndAlso IsCheck2 = True Then
            IsChecksum = True

            ' byref
            Key1 = GetByte(ToInt32(PacketDic((ToInt32(ExitBits(0)) << 8) + Key1)) + Key1)

        End If

        Dim res As New DecryptResult(Combine(OpBits, ConBits), IsChecksum)

        Return res

    End Function
    Public Function Packet_Decrypt_FromServer(ByRef Packet() As Byte, ByRef Key1 As Byte) As DecryptResult

        Dim OpBits(8) As Byte, Key2 As Int32
        OpBits(4) = PacketDic((ToInt32(Key1 Xor Packet(2)) << 8) + Packet(4))
        OpBits(5) = PacketDic((ToInt32(Packet(7) Xor Packet(3)) << 8) + Packet(5))
        OpBits(2) = PacketDic((ToInt32(Packet(0) Xor Packet(6)) << 8) + Packet(2))
        OpBits(3) = PacketDic((ToInt32(Key1 Xor Packet(1)) << 8) + Packet(3))
        OpBits(0) = PacketDic((ToInt32(Key1 Xor Packet(7)) << 8) + Packet(0))
        OpBits(1) = PacketDic((ToInt32(Packet(7) Xor Packet(6)) << 8) + Packet(1))
        OpBits(7) = PacketDic((ToInt32(Packet(6)) << 8) + Packet(7)) Xor 2
        OpBits(6) = Packet(6)

        Dim Ischeck_Header As Boolean = False
        Dim v3 As Byte = PacketDic(PacketDic((ToInt32(OpBits(0)) << 8) + OpBits(2)) + (ToInt32(Key1) << 8))
        Dim v6 As Byte = PacketDic(PacketDic((ToInt32(OpBits(3)) << 8) + OpBits(1)) + (ToInt32(OpBits(6)) << 8))
        OpBits(8) = PacketDic((ToInt32(v3) << 8) + v6)
        Key2 = (ToInt32(OpBits(3)) << 8) + OpBits(2)

        If OpBits(8) = Packet(8) Then
            Ischeck_Header = True
        End If

        Dim CKey As Int32, ConBits() As Byte
        CKey = ToInt32(OpBits(6)) * Key2
        ReDim ConBits(UBound(Packet) - 9 - 2)

        Dim a As Integer = 8, PKey As Int64
        For i As Integer = 0 To UBound(ConBits)
            a = 9 + i
            ConBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor i)) << 8) + Packet(a))
            PKey += ConBits(i)
        Next

        Dim ExitBits() As Byte
        ReDim ExitBits(1)
        For i As Integer = 0 To UBound(ExitBits)
            a += 1
            ExitBits(i) = PacketDic((ToInt32(GetByte(GetByte(CKey) Xor (a - 9))) << 8) + Packet(a))
        Next

        Dim pPair As Int32 = (ToInt32(ExitBits(1)) << 8) + ExitBits(0)
        Dim IsCheck1 As Boolean = False

        If PKey = pPair Then
            IsCheck1 = True
        End If

        Dim IsChecksum As Boolean = False
        If Ischeck_Header = True And IsCheck1 = True Then
            IsChecksum = True
        End If

        Key1 = GetByte(Key1 + ToInt32(PacketDic((ToInt32(ExitBits(0)) << 8) + Key1)))

        Dim res As New DecryptResult(Combine(OpBits, ConBits), IsChecksum)

        Return res

    End Function
    Public Function CtB(ByRef Str As String) As Byte()
        Return CharToBytes(Str)
    End Function
    Public Function CharToBytes(ByRef Str As String) As Byte()
        Return System.Text.Encoding.UTF8.GetBytes(Str)
    End Function
    Public Function CP949ToBytes(ByRef Str As String) As Byte()
        Return System.Text.Encoding.GetEncoding(949).GetBytes(Str)
    End Function
    Public Function BytesTo949(ByRef Bytes() As Byte) As String
        Return System.Text.Encoding.GetEncoding(949).GetString(Bytes)
    End Function
    Public Function BytesToStr(ByRef Bytes() As Byte) As String
        Return System.Text.Encoding.UTF8.GetString(Bytes)
    End Function
    Public Function ByteLen(Str As String) As Integer
        Return (UBound(CP949ToBytes(Str)) + 1)
    End Function
    Public Function Reverse(ByRef Bytes() As Byte) As Byte()
        Dim NByte() As Byte = Bytes
        Array.Reverse(NByte)
        Return NByte
    End Function
    Public Function GetHeader(ByRef Header As PacketHeader, ByRef Key1 As Byte) As Byte()
        Dim HeaderData(8) As Byte
        HeaderData(0) = LOBYTE(Header.PacketLength)
        HeaderData(1) = HIBYTE(Header.PacketLength)
        HeaderData(2) = LOBYTE(Header.OpCode)
        HeaderData(3) = HIBYTE(Header.OpCode)
        HeaderData(4) = Header.ZeroBits(0)
        HeaderData(5) = Header.ZeroBits(1)
        HeaderData(6) = Header.Rand1
        HeaderData(7) = Header.Constant_Byte
        Return HeaderData
    End Function
    Public Function ByteToUInt32(ByRef Bytes() As Byte) As UInt32
        Return BitConverter.ToUInt32(Bytes, 0)
    End Function
    Public Function IntToByte(ByRef Int As UInt16) As Byte()
        Return BitConverter.GetBytes(Int)
    End Function
    Public Function IntToByte(ByRef Int As UInt32) As Byte()
        Return BitConverter.GetBytes(Int)
    End Function
    Public Function ReadPacket_Bytes(ByRef Bytes() As Byte, ReadCount As Integer) As ReadPacketResult

        Dim OldBytes() As Byte = {}, NewBytes() As Byte = {}
        Dim result As ReadPacketResult
        result = New ReadPacketResult With {
                    .HasError = True,
                    .ReadBytes = {},
                    .LeftBytes = {}
                    }

        If UBound(Bytes) < (ReadCount - 1) Then
            Return result
        End If

        ReDim OldBytes(ReadCount - 1)
        ReDim NewBytes(UBound(Bytes) - (UBound(OldBytes) + 1))

        For i As Integer = 0 To UBound(Bytes)
            If i <= UBound(OldBytes) Then
                OldBytes(i) = Bytes(i)
            Else
                NewBytes(i - (UBound(OldBytes) + 1)) = Bytes(i)
            End If
        Next

        result = New ReadPacketResult With {
            .HasError = False,
            .ReadBytes = OldBytes,
            .LeftBytes = NewBytes
            }

        Return result

    End Function
    Public Function ReadPacket_String(ByRef Bytes() As Byte) As ReadPacketResult

        Dim OldBytes() As Byte = {}, NewBytes() As Byte = {}, CutCount As Integer = -1
        Dim result As ReadPacketResult
        result = New ReadPacketResult With {
                    .HasError = True,
                    .ReadBytes = {},
                    .LeftBytes = {}
                    }

        For i As Integer = 0 To UBound(Bytes)
            If Bytes(i) = 0 Then
                ReDim OldBytes(i - 1)
                ReDim NewBytes(UBound(Bytes) - UBound(OldBytes) - 1)

                If UBound(OldBytes) = -1 Then
                    Return result
                Else
                    CutCount = i
                    Exit For
                End If
            End If
        Next

        If CutCount = -1 Then
            Return result
        End If

        For n As Integer = 0 To UBound(OldBytes) + UBound(NewBytes)
            If n <= UBound(OldBytes) Then
                OldBytes(n) = Bytes(n)
            Else
                NewBytes(n - UBound(OldBytes) - 1) = Bytes(n + 1)
            End If
        Next

        result = New ReadPacketResult With {
            .HasError = False,
            .ReadBytes = OldBytes,
            .LeftBytes = NewBytes
            }
        Return result

    End Function
    Public Function ReadByte(ByRef Bytes() As Byte) As Byte
        Return ReadBytes(Bytes, 1)(0)
    End Function
    Public Function ReadBytes(ByRef Bytes() As Byte, count As Integer) As Byte()
        Dim readresult As ReadPacketResult = ReadPacket_Bytes(Bytes, count)
        If readresult.HasError = True Then
            Throw New Exception("ReadBytes Error")
        End If
        Bytes = readresult.LeftBytes
        Return readresult.ReadBytes
    End Function
    Public Function ReadString(ByRef Bytes() As Byte) As String
        Dim readresult As ReadPacketResult = ReadPacket_String(Bytes)
        If readresult.HasError = True Then
            Throw New Exception("ReadString Error")
        End If
        Bytes = readresult.LeftBytes
        Return BytesTo949(readresult.ReadBytes)
    End Function
    Public Function Combine_PacketHeader(ByRef Header As PacketHeader, Content() As Byte, ByRef Key1 As Byte) As Byte()

        Dim HeaderData() As Byte = GetHeader(Header, Key1)

        Return Combine(HeaderData, Content)

    End Function


End Module

Public Class ReadPacketResult
    Public HasError As Boolean
    Public ReadBytes() As Byte
    Public LeftBytes() As Byte
End Class

Public Class DecryptResult
    Public Header As PacketHeader
    Public ContentBytes As Byte()
    Public Checksum As Boolean
    Public Sub New(ByRef Packets As Byte(), ByRef IsChecksum As Boolean)

        Dim HeaderBytes(8) As Byte
        ReDim ContentBytes(UBound(Packets) - 9)

        For i As Integer = 0 To UBound(Packets)
            If i < 9 Then
                HeaderBytes(i) = Packets(i)
            Else
                ContentBytes(i - 9) = Packets(i)
            End If
        Next

        Header = New PacketHeader With {
                    .PacketLength = ToUInt16((ToUInt16(HeaderBytes(1)) << 8) + HeaderBytes(0)),
                    .OpCode = ToUInt16((ToUInt16(HeaderBytes(3)) << 8) + HeaderBytes(2)),
                    .ZeroBits = {HeaderBytes(4), HeaderBytes(5)},
                    .Rand1 = HeaderBytes(6),
                    .Constant_Byte = HeaderBytes(7),
                    .Rand2 = HeaderBytes(8)
                }

        Checksum = IsChecksum
    End Sub
End Class

Public Class PacketHeader
    Public PacketLength As UInt16 ' 2 Bytes
    Public OpCode As UInt16 ' 2 Bytes
    Public ZeroBits() As Byte = {&H0, &H0} ' 2 Bytes
    Public Rand1 As Byte
    Public Constant_Byte As Byte
    Public Rand2 As Byte
End Class